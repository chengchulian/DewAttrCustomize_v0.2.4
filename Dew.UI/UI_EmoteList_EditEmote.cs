using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EmoteList_EditEmote : SingletonBehaviour<UI_EmoteList_EditEmote>
{
	public UI_EmoteList_EditEmoteWheel_Item[] wheelItems;

	public Button saveButton;

	public Button revertButton;

	public GameObject dirtyObject;

	public SafeAction onItemsUpdated;

	public GameObject gamepadActionDisplay;

	public GameObject[] draggingObjects;

	public GameObject[] notDraggingObjects;

	[NonSerialized]
	private List<string> _emotesState = new List<string>();

	public IReadOnlyList<string> emotesState => _emotesState;

	private void Start()
	{
		ManagerBase<GlobalUIManager>.instance.AddBackHandler(this, 1000, delegate
		{
			if (DewInput.currentMode == InputMode.KeyboardAndMouse || ManagerBase<GlobalUIManager>.instance.focused == null || !ManagerBase<GlobalUIManager>.instance.focused.GetTransform().TryGetComponent<UI_EmoteList_EditEmoteWheel_Item>(out var component))
			{
				return false;
			}
			SetItem(component.index, null);
			return true;
		});
		Revert();
		UpdateItems();
		saveButton.onClick.AddListener(delegate
		{
			DewSave.profile.equippedEmotes.Clear();
			DewSave.profile.equippedEmotes.AddRange(_emotesState);
			DewSave.SaveProfile();
			UpdateDirty();
		});
		revertButton.onClick.AddListener(Revert);
	}

	private void Update()
	{
		gamepadActionDisplay.SetActive(DewInput.currentMode == InputMode.Gamepad && ManagerBase<GlobalUIManager>.instance.focused != null && ManagerBase<GlobalUIManager>.instance.focused.GetTransform().GetComponent<UI_EmoteList_EditEmoteWheel_Item>() != null);
		UI_EmoteList.DraggingInfo di = SingletonBehaviour<UI_EmoteList>.instance.draggingInfo;
		draggingObjects.SetActiveAll(di.isDragging);
		notDraggingObjects.SetActiveAll(!di.isDragging);
	}

	public void SetItem(int slotIndex, string emoteName)
	{
		_emotesState[slotIndex] = emoteName;
		UpdateItems();
		UpdateDirty();
	}

	public void Revert()
	{
		_emotesState.Clear();
		_emotesState.AddRange(DewSave.profile.equippedEmotes);
		UpdateItems();
		UpdateDirty();
	}

	private void UpdateDirty()
	{
		for (int i = 0; i < _emotesState.Count; i++)
		{
			if (_emotesState[i] != DewSave.profile.equippedEmotes[i])
			{
				dirtyObject.SetActive(value: true);
				return;
			}
		}
		dirtyObject.SetActive(value: false);
	}

	private void UpdateItems()
	{
		for (int i = 0; i < wheelItems.Length; i++)
		{
			wheelItems[i].index = i;
			if (i >= DewSave.profile.equippedEmotes.Count)
			{
				wheelItems[i].Setup(null);
			}
			else
			{
				wheelItems[i].Setup(_emotesState[i]);
			}
		}
		onItemsUpdated?.Invoke();
	}
}
