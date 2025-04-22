using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EmoteList : SingletonBehaviour<UI_EmoteList>
{
	public struct DraggingInfo
	{
		public bool isDragging;

		public int slotIndex;

		public int listIndex;
	}

	public UI_EmoteList_Item itemPrefab;

	public RectTransform itemsParent;

	public RectTransform previewParent;

	public GameObject gamepadListBlocker;

	public GameObject fxStartDrag;

	public GameObject fxEndDrag;

	public TextMeshProUGUI progressText;

	public GameObject[] disableIfNoneExists;

	public GameObject newObject;

	public DraggingInfo draggingInfo;

	public SafeAction<DraggingInfo> onDraggingInfoChanged;

	private UI_ToggleGroup _toggleGroup;

	[NonSerialized]
	public List<UI_EmoteList_Item> shownItems = new List<UI_EmoteList_Item>();

	private void Start()
	{
		_toggleGroup = GetComponentInChildren<UI_ToggleGroup>();
		_toggleGroup.currentIndex = -1;
		_toggleGroup.onCurrentIndexChanged.AddListener(delegate
		{
			if (!(previewParent == null) && _toggleGroup.currentIndex > 0)
			{
				for (int num = previewParent.childCount - 1; num >= 0; num--)
				{
					global::UnityEngine.Object.Destroy(previewParent.GetChild(num).gameObject);
				}
				global::UnityEngine.Object.Instantiate(shownItems[_toggleGroup.currentIndex].currentEmotePrefab, previewParent).SetupPreview();
			}
		});
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Combine(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, 2000, delegate
		{
			if (DewInput.currentMode == InputMode.KeyboardAndMouse || !draggingInfo.isDragging)
			{
				return false;
			}
			if (draggingInfo.listIndex >= 0)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(shownItems[draggingInfo.listIndex].GetComponent<IGamepadFocusable>());
			}
			else
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(SingletonBehaviour<UI_EmoteList_EditEmote>.instance.wheelItems[draggingInfo.slotIndex].GetComponent<IGamepadFocusable>());
			}
			DropCancel();
			return true;
		});
		RefreshList();
		onDraggingInfoChanged += (Action<DraggingInfo>)delegate
		{
			if (gamepadListBlocker != null)
			{
				gamepadListBlocker.SetActive(draggingInfo.isDragging && DewInput.currentMode == InputMode.Gamepad);
			}
			if (draggingInfo.isDragging)
			{
				DewEffect.Play(fxStartDrag);
			}
			else
			{
				DewEffect.Play(fxEndDrag);
			}
		};
		gamepadListBlocker.SetActive(value: false);
	}

	private void OnDestroy()
	{
		DewInput.onCurrentModeChanged = (Action<InputMode, InputMode>)Delegate.Remove(DewInput.onCurrentModeChanged, new Action<InputMode, InputMode>(OnCurrentModeChanged));
	}

	private void OnCurrentModeChanged(InputMode arg1, InputMode arg2)
	{
		DropCancel();
	}

	public void RefreshList()
	{
		foreach (UI_EmoteList_Item shownItem in shownItems)
		{
			global::UnityEngine.Object.Destroy(shownItem.gameObject);
		}
		shownItems.Clear();
		int unlocked = 0;
		List<(Emote, DewProfile.CosmeticsData)> list = new List<(Emote, DewProfile.CosmeticsData)>();
		foreach (KeyValuePair<string, DewProfile.CosmeticsData> e in DewSave.profile.emotes)
		{
			Emote emote = DewResources.GetByName<Emote>(e.Key);
			if (!(emote == null))
			{
				list.Add((emote, e.Value));
			}
		}
		list.Sort(((Emote, DewProfile.CosmeticsData) t0, (Emote, DewProfile.CosmeticsData) t1) => string.Compare(t0.Item1.name, t1.Item1.name, StringComparison.Ordinal));
		for (int i = 0; i < list.Count; i++)
		{
			(Emote, DewProfile.CosmeticsData) t2 = list[i];
			UI_EmoteList_Item newItem = global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent);
			newItem.Setup(t2.Item1.name);
			newItem.GetComponentInChildren<UI_Toggle>().index = i;
			shownItems.Add(newItem);
			if (t2.Item2.isUnlocked)
			{
				unlocked++;
			}
		}
		if (progressText != null)
		{
			progressText.text = $"{unlocked}/{list.Count}";
		}
		if (disableIfNoneExists != null)
		{
			disableIfNoneExists.SetActiveAll(list.Count > 0 && !DewBuildProfile.current.HasFeature(BuildFeatureTag.Booth));
		}
		UpdateNewStatus();
	}

	public void StartDragListItem(int index)
	{
		draggingInfo = new DraggingInfo
		{
			isDragging = true,
			listIndex = index,
			slotIndex = -1
		};
		onDraggingInfoChanged?.Invoke(draggingInfo);
	}

	public void StartDragSlotItem(int index)
	{
		draggingInfo = new DraggingInfo
		{
			isDragging = true,
			listIndex = -1,
			slotIndex = index
		};
		onDraggingInfoChanged?.Invoke(draggingInfo);
	}

	public void DropSmart()
	{
		ListReturnHandle<RaycastResult> handle;
		List<RaycastResult> list = Dew.RaycastAllUIElementsBelowCursor(out handle);
		bool dontDrop = false;
		foreach (RaycastResult r in list)
		{
			if (r.gameObject.GetComponentInParent<UI_EmoteList_EditEmote>() != null)
			{
				dontDrop = true;
			}
			UI_EmoteList_EditEmoteWheel_Item wheelItem = r.gameObject.GetComponentInParent<UI_EmoteList_EditEmoteWheel_Item>();
			if (!(wheelItem == null))
			{
				DropOnSlot(wheelItem.index);
				handle.Return();
				return;
			}
		}
		handle.Return();
		if (!dontDrop && draggingInfo.slotIndex >= 0)
		{
			SingletonBehaviour<UI_EmoteList_EditEmote>.instance.SetItem(draggingInfo.slotIndex, null);
		}
		draggingInfo = default(DraggingInfo);
		onDraggingInfoChanged?.Invoke(draggingInfo);
	}

	public void DropCancel()
	{
		if (draggingInfo.isDragging)
		{
			draggingInfo = default(DraggingInfo);
			onDraggingInfoChanged?.Invoke(draggingInfo);
		}
	}

	public void DropOnSlot(int index)
	{
		if (draggingInfo.isDragging)
		{
			if (draggingInfo.listIndex >= 0)
			{
				SingletonBehaviour<UI_EmoteList_EditEmote>.instance.SetItem(index, shownItems[draggingInfo.listIndex].currentEmoteName);
			}
			else
			{
				string a = SingletonBehaviour<UI_EmoteList_EditEmote>.instance.emotesState[draggingInfo.slotIndex];
				string b = SingletonBehaviour<UI_EmoteList_EditEmote>.instance.emotesState[index];
				SingletonBehaviour<UI_EmoteList_EditEmote>.instance.SetItem(draggingInfo.slotIndex, b);
				SingletonBehaviour<UI_EmoteList_EditEmote>.instance.SetItem(index, a);
			}
			draggingInfo = default(DraggingInfo);
			onDraggingInfoChanged?.Invoke(draggingInfo);
		}
	}

	public void UpdateNewStatus()
	{
		if (newObject == null)
		{
			return;
		}
		newObject.SetActive(value: false);
		foreach (KeyValuePair<string, DewProfile.CosmeticsData> emote in DewSave.profile.emotes)
		{
			if (emote.Value.isNew)
			{
				newObject.SetActive(value: true);
			}
		}
	}
}
