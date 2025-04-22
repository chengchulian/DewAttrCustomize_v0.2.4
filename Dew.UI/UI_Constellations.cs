using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Constellations : SingletonBehaviour<UI_Constellations>
{
	public GameObject unselectHitboxObject;

	public GameObject fxSelectGroup;

	public GameObject fxUnselectGroup;

	public GameObject fxHoverGroup;

	public GameObject fxUndoChanges;

	public GameObject fxApplyChanges;

	public Action<UI_Constellations_CategoryGroup> onSelectedGroupChanged;

	public Action<UI_Constellations_CategoryGroup> onHoveredGroupChanged;

	public Action onStateChanged;

	public ConstellationState state;

	public GameObject dirtyObject;

	public Button resetButton;

	public TextMeshProUGUI costText;

	public Color sufficientColor;

	public Color insufficientColor;

	private View _view;

	private UI_Constellations_CategoryGroup[] _groups;

	private UI_Constellations_CategoryGroup[] _heroGroups;

	public UI_Constellations_CategoryGroup selectedGroup { get; private set; }

	public UI_Constellations_CategoryGroup hoveredGroup { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		_view = GetComponentInParent<View>();
		_view.onShow.AddListener(ResetState);
		_groups = GetComponentsInChildren<UI_Constellations_CategoryGroup>(includeInactive: true);
		_heroGroups = _groups.Where((UI_Constellations_CategoryGroup g) => g.name.StartsWith("Hero_")).ToArray();
		ResetState();
		onStateChanged = (Action)Delegate.Combine(onStateChanged, new Action(UpdateDirtyObjectState));
		onSelectedGroupChanged = (Action<UI_Constellations_CategoryGroup>)Delegate.Combine(onSelectedGroupChanged, new Action<UI_Constellations_CategoryGroup>(OnSelectedGroupChanged));
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Combine(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void OnDestroy()
	{
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Remove(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void OnSelectedGroupChanged(UI_Constellations_CategoryGroup obj)
	{
		UpdateDirtyObjectState();
	}

	private void OnCurrentModeChanged(InputMode arg1, InputMode arg2)
	{
		UpdateDirtyObjectState();
	}

	private void UpdateDirtyObjectState()
	{
		dirtyObject.SetActive(state.isDirty && (selectedGroup == null || DewInput.currentMode == InputMode.KeyboardAndMouse));
		resetButton.gameObject.SetActive(!state.isDirty && DewSave.profile.stars.Any((KeyValuePair<string, DewProfile.StarData> s) => s.Value.level > 0));
		if (resetButton.gameObject.activeSelf)
		{
			int cost = GetConstellationsResetCost();
			bool canAfford = DewSave.profile.stardust >= cost;
			costText.text = cost.ToString("#,##0");
			costText.color = (canAfford ? sufficientColor : insufficientColor);
			resetButton.interactable = canAfford;
		}
	}

	public void ClickResetConstellations()
	{
		int cost = GetConstellationsResetCost();
		if (DewSave.profile.stardust < cost)
		{
			return;
		}
		ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
		{
			owner = this,
			buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
			validator = () => LobbyUIManager.instance != null && LobbyUIManager.instance.IsState("Character"),
			defaultButton = DewMessageSettings.ButtonType.Cancel,
			destructiveConfirm = true,
			rawContent = string.Format(DewLocalization.GetUIValue("Constellations_Reset_Confirmation"), cost),
			onClose = delegate(DewMessageSettings.ButtonType b)
			{
				int refunded;
				if (b == DewMessageSettings.ButtonType.Yes)
				{
					DewSave.profile.stardust -= cost;
					if (DewSave.profile.spentStardust == 0)
					{
						foreach (KeyValuePair<string, DewProfile.StarData> current in DewSave.profile.stars)
						{
							DewSave.profile.spentStardust += current.Value.level * 40;
						}
					}
					foreach (KeyValuePair<string, DewProfile.StarData> star in DewSave.profile.stars)
					{
						star.Value.level = 0;
					}
					refunded = DewSave.profile.spentStardust;
					DewSave.profile.stardust += refunded;
					DewSave.profile.spentStardust = 0;
					DewSave.SaveProfile();
					ResetState();
					DewEffect.Play(fxApplyChanges);
					StartCoroutine(Routine());
				}
				IEnumerator Routine()
				{
					yield return new WaitForSeconds(0.5f);
					ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
					{
						owner = this,
						buttons = DewMessageSettings.ButtonType.Ok,
						rawContent = string.Format(DewLocalization.GetUIValue("Constellations_Reset_Done"), refunded)
					});
				}
			}
		});
	}

	private int GetConstellationsResetCost()
	{
		int sum = 0;
		foreach (KeyValuePair<string, DewProfile.StarData> star in DewSave.profile.stars)
		{
			sum += star.Value.level;
		}
		if (sum < 10)
		{
			return 5;
		}
		if (sum < 25)
		{
			return 10;
		}
		if (sum < 50)
		{
			return 15;
		}
		return 20;
	}

	private void Start()
	{
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, 100, delegate
		{
			if (!_view.isShowing)
			{
				return false;
			}
			if (selectedGroup != null)
			{
				SelectGroup(null);
				return true;
			}
			if (state.isDirty)
			{
				GoBackWithUndoConfirm();
				return true;
			}
			return false;
		});
		unselectHitboxObject.SetActive(value: false);
		UI_Constellations_CategoryGroup[] heroGroups = _heroGroups;
		foreach (UI_Constellations_CategoryGroup obj in heroGroups)
		{
			obj.gameObject.SetActive(value: true);
			obj.gameObject.SetActive(value: false);
		}
		NetworkedManagerBase<PlayLobbyManager>.instance.ClientEvent_OnLocalPlayerHeroChanged += (Action<string>)delegate
		{
			ResetState();
		};
		if (DewSave.profile.spentStardust != 0)
		{
			return;
		}
		UI_Constellations_StarItem[] componentsInChildren = GetComponentsInChildren<UI_Constellations_StarItem>(includeInactive: true);
		foreach (UI_Constellations_StarItem c in componentsInChildren)
		{
			for (int j = 0; j < c.data.level; j++)
			{
				DewSave.profile.spentStardust += c.requiredStardusts[j];
			}
		}
		if (DewSave.profile.spentStardust > 0)
		{
			Debug.Log("Detected older save. Calculated spentStardust: " + DewSave.profile.spentStardust);
		}
	}

	public void GoBackWithUndoConfirm()
	{
		if (LobbyUIManager.instance.IsState("Character"))
		{
			DoActionWithUndoConfirm(delegate
			{
				LobbyUIManager.instance.SetState("Lobby");
			});
		}
	}

	public void DoActionWithUndoConfirm(Action callback)
	{
		if (state.isDirty)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				owner = this,
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.No),
				defaultButton = DewMessageSettings.ButtonType.No,
				validator = () => _view.isShowing,
				destructiveConfirm = true,
				rawContent = DewLocalization.GetUIValue("Constellations_Message_DiscardUnsavedChanges"),
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b == DewMessageSettings.ButtonType.Yes)
					{
						ResetState();
						callback?.Invoke();
					}
				}
			});
		}
		else
		{
			callback?.Invoke();
		}
	}

	public void UndoChanges()
	{
		DewEffect.Play(fxUndoChanges);
		ResetState();
		ManagerBase<GlobalUIManager>.instance.TryMoveFocusBack();
	}

	private void ResetState()
	{
		if (_heroGroups != null && DewPlayer.local != null)
		{
			UI_Constellations_CategoryGroup[] heroGroups = _heroGroups;
			foreach (UI_Constellations_CategoryGroup h in heroGroups)
			{
				h.gameObject.SetActive(h.name == DewPlayer.local.selectedHeroType);
			}
		}
		state = new ConstellationState
		{
			isDirty = false,
			stardust = DewSave.profile.stardust,
			stars = new Dictionary<string, DewProfile.StarData>()
		};
		foreach (KeyValuePair<string, DewProfile.StarData> p in DewSave.profile.stars)
		{
			state.stars.Add(p.Key, p.Value.Clone());
		}
		onStateChanged?.Invoke();
	}

	public void SetDirtyAndInvokeChanged(bool needsToCheckUnDirty = false)
	{
		state.isDirty = true;
		if (needsToCheckUnDirty && state.stardust == DewSave.profile.stardust)
		{
			state.isDirty = false;
			foreach (KeyValuePair<string, DewProfile.StarData> s in state.stars)
			{
				if (!s.Value.IsUnchanged(DewSave.profile.stars[s.Key]))
				{
					state.isDirty = true;
					break;
				}
			}
		}
		onStateChanged?.Invoke();
	}

	public void CommitState()
	{
		DewEffect.Play(fxApplyChanges);
		DewSave.profile.spentStardust += DewSave.profile.stardust - state.stardust;
		Debug.Log($"Spent {DewSave.profile.stardust - state.stardust} stardust, in total {DewSave.profile.spentStardust}");
		DewSave.profile.stardust = state.stardust;
		DewSave.profile.stars = state.stars;
		DewSave.SaveProfile();
		ResetState();
		UI_Constellations_CategoryGroup[] groups = _groups;
		foreach (UI_Constellations_CategoryGroup g in groups)
		{
			if (g.isActiveAndEnabled)
			{
				g.Flash(2f);
			}
		}
		if (selectedGroup != null)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.45f);
			if (selectedGroup != null && this != null && _view.isShowing)
			{
				Unselect();
			}
		}
	}

	public void SelectGroup(UI_Constellations_CategoryGroup group)
	{
		if (group != null && hoveredGroup != null)
		{
			HoverGroup(null);
		}
		if (selectedGroup == group)
		{
			return;
		}
		if (selectedGroup != null && group == null && DewInput.currentMode == InputMode.Gamepad)
		{
			UI_Constellations_CategoryGroup prev = selectedGroup;
			Dew.CallDelayed(delegate
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(prev);
			});
		}
		selectedGroup = group;
		onSelectedGroupChanged?.Invoke(group);
		unselectHitboxObject.SetActive(group != null);
		DewEffect.Play((group == null) ? fxUnselectGroup : fxSelectGroup);
		if (DewSave.profile.didReadConstellationNotice)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				owner = this,
				buttons = DewMessageSettings.ButtonType.Ok,
				rawContent = DewLocalization.GetUIValue("Constellations_Message_Intro_" + i),
				onClose = ((i != 2) ? null : ((Action<DewMessageSettings.ButtonType>)delegate
				{
					DewSave.profile.didReadConstellationNotice = true;
					DewSave.SaveProfile();
				}))
			});
		}
	}

	public void Unselect()
	{
		SelectGroup(null);
	}

	public void HoverGroup(UI_Constellations_CategoryGroup group)
	{
		if (!(selectedGroup != null) && !(hoveredGroup == group))
		{
			hoveredGroup = group;
			onHoveredGroupChanged?.Invoke(group);
			if (group != null)
			{
				DewEffect.Play(fxHoverGroup);
			}
		}
	}
}
