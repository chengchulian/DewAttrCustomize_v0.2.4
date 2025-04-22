using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_Constellations : SingletonBehaviour<UI_Lobby_Constellations>
{
	public SafeAction onLoadoutChanged;

	public SafeAction onIsDraggingChanged;

	public Transform loadoutToggleParent;

	public GameObject fxDragStart;

	public GameObject[] fxDragStartFlavourByType;

	public GameObject fxDragEndDiscarded;

	public GameObject fxDragEndEquipped;

	public GameObject fxSaveChanges;

	public GameObject fxResetChanges;

	public GameObject dirtyObject;

	public Button saveButton;

	public Button resetButton;

	public HeroLoadoutData loadout { get; private set; }

	public int selectedLoadoutIndex { get; private set; }

	public bool isDirty { get; private set; }

	public bool isDragging => draggingStar != null;

	public bool isDraggingFromSlot { get; private set; }

	public StarEffect draggingStar { get; private set; }

	public StarType draggingSlotType { get; private set; }

	public int draggingSlotIndex { get; private set; }

	private void Start()
	{
		UI_Toggle[] componentsInChildren = loadoutToggleParent.GetComponentsInChildren<UI_Toggle>();
		foreach (UI_Toggle t in componentsInChildren)
		{
			UI_Toggle toggle = t;
			toggle.doNotToggleOnClick = true;
			toggle.onClick.AddListener(delegate
			{
				ClickOnLoadout(toggle.index);
			});
		}
		onLoadoutChanged += (Action)delegate
		{
			dirtyObject.SetActive(isDirty);
		};
		saveButton.onClick.AddListener(Save);
		resetButton.onClick.AddListener(ResetChanges);
		dirtyObject.SetActive(value: false);
	}

	private void OnEnable()
	{
		if (!(DewPlayer.local == null))
		{
			string h = DewPlayer.local.selectedHeroType;
			LoadLoadout(DewSave.profile.heroSelectedLoadoutIndex[h]);
		}
	}

	public void ClickOnLoadout(int index)
	{
		if (index == selectedLoadoutIndex)
		{
			return;
		}
		string h = DewPlayer.local.selectedHeroType;
		if (index < 0 || index >= DewSave.profile.heroLoadouts[h].Count)
		{
			return;
		}
		if (isDirty)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
				defaultButton = DewMessageSettings.ButtonType.Cancel,
				owner = this,
				rawContent = DewLocalization.GetUIValue("Constellations_DiscardUnsavedChanges"),
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b != DewMessageSettings.ButtonType.Cancel)
					{
						LoadLoadout(index);
					}
				},
				destructiveConfirm = true
			});
		}
		else
		{
			LoadLoadout(index);
			DewSave.profile.heroSelectedLoadoutIndex[h] = index;
			DewPlayer.local.CmdSetHeroLoadoutData(loadout);
		}
	}

	public void ClickOnGoBack()
	{
		if (isDirty)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
				defaultButton = DewMessageSettings.ButtonType.Cancel,
				owner = this,
				rawContent = DewLocalization.GetUIValue("Constellations_DiscardUnsavedChanges"),
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b != DewMessageSettings.ButtonType.Cancel)
					{
						LobbyUIManager.instance.SetState("Character");
					}
				},
				destructiveConfirm = true
			});
		}
		else
		{
			LobbyUIManager.instance.SetState("Character");
		}
	}

	public void NotifyChange()
	{
		isDirty = true;
		loadout.Validate_Imp(DewPlayer.local.selectedHeroType, isRepair: true, checkStarLevels: false, DewSave.profile.heroUnlockedStarSlots[DewPlayer.local.selectedHeroType]);
		onLoadoutChanged?.Invoke();
	}

	public void LoadLoadout(int index)
	{
		string h = DewPlayer.local.selectedHeroType;
		selectedLoadoutIndex = index;
		loadout = new HeroLoadoutData(DewSave.profile.heroLoadouts[h][selectedLoadoutIndex]);
		isDirty = false;
		onLoadoutChanged?.Invoke();
		UI_Toggle[] componentsInChildren = loadoutToggleParent.GetComponentsInChildren<UI_Toggle>();
		foreach (UI_Toggle obj in componentsInChildren)
		{
			obj.isChecked = obj.index == index;
		}
	}

	public void Save()
	{
		DewEffect.PlayNew(fxSaveChanges);
		DewSave.profile.heroLoadouts[DewPlayer.local.selectedHeroType][selectedLoadoutIndex] = loadout;
		LoadLoadout(selectedLoadoutIndex);
		DewSave.SaveProfile();
		DewPlayer.local.CmdSetHeroLoadoutData(loadout);
	}

	public void ResetChanges()
	{
		DewEffect.PlayNew(fxResetChanges);
		LoadLoadout(selectedLoadoutIndex);
	}

	public void StartDragStar(StarEffect star)
	{
		DewEffect.PlayNew(fxDragStart);
		DewEffect.PlayNew(fxDragStartFlavourByType[(int)star.type]);
		draggingStar = star;
		isDraggingFromSlot = false;
		onIsDraggingChanged?.Invoke();
	}

	public void StartDraggingSlot(StarType type, int index, StarEffect star)
	{
		DewEffect.PlayNew(fxDragStart);
		DewEffect.PlayNew(fxDragStartFlavourByType[(int)star.type]);
		draggingStar = star;
		draggingSlotIndex = index;
		draggingSlotType = type;
		isDraggingFromSlot = true;
		onIsDraggingChanged?.Invoke();
	}

	public void EndDrag()
	{
		if (draggingStar == null)
		{
			return;
		}
		UI_Lobby_Constellations_HeroConstellation_StarSlot dropTarget = Dew.GetUIComponentBelowCursor<UI_Lobby_Constellations_HeroConstellation_StarSlot>();
		if (isDraggingFromSlot)
		{
			if (dropTarget != null)
			{
				if (dropTarget.isLocked)
				{
					DewEffect.PlayNew(fxDragEndDiscarded);
				}
				else
				{
					List<LoadoutStarItem> starList = loadout.GetStarList(draggingSlotType);
					List<LoadoutStarItem> toList = loadout.GetStarList(dropTarget.type);
					List<LoadoutStarItem> list = toList;
					int index = dropTarget.index;
					List<LoadoutStarItem> list2 = starList;
					int index2 = draggingSlotIndex;
					LoadoutStarItem loadoutStarItem = starList[draggingSlotIndex];
					LoadoutStarItem loadoutStarItem2 = toList[dropTarget.index];
					LoadoutStarItem loadoutStarItem4 = (list[index] = loadoutStarItem);
					loadoutStarItem4 = (list2[index2] = loadoutStarItem2);
					DewEffect.PlayNew(fxDragEndEquipped);
				}
			}
			else
			{
				loadout.GetStarList(draggingSlotType)[draggingSlotIndex] = default(LoadoutStarItem);
				DewEffect.PlayNew(fxDragEndDiscarded);
			}
			NotifyChange();
		}
		else if (dropTarget != null)
		{
			RemoveDuplicate(StarType.Destruction);
			RemoveDuplicate(StarType.Life);
			RemoveDuplicate(StarType.Imagination);
			RemoveDuplicate(StarType.Flexible);
			loadout.GetStarList(dropTarget.type)[dropTarget.index] = new LoadoutStarItem
			{
				name = draggingStar.GetType().Name
			};
			DewEffect.PlayNew(fxDragEndEquipped);
			NotifyChange();
		}
		else
		{
			DewEffect.PlayNew(fxDragEndDiscarded);
		}
		draggingStar = null;
		onIsDraggingChanged?.Invoke();
		void RemoveDuplicate(StarType type)
		{
			List<LoadoutStarItem> list3 = loadout.GetStarList(type);
			for (int i = 0; i < list3.Count; i++)
			{
				if (list3[i].name == draggingStar.GetType().Name)
				{
					list3[i] = default(LoadoutStarItem);
				}
			}
		}
	}
}
