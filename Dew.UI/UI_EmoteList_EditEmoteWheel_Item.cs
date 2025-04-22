using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EmoteList_EditEmoteWheel_Item : UI_StationaryEmote, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
	public GameObject highlightObject;

	public GameObject nonHighlightObject;

	public GameObject draggingObject;

	[NonSerialized]
	public int index;

	private void Start()
	{
		draggingObject.SetActive(value: false);
		SingletonBehaviour<UI_EmoteList>.instance.onDraggingInfoChanged += new Action<UI_EmoteList.DraggingInfo>(OnDraggingInfoChanged);
	}

	private void OnDraggingInfoChanged(UI_EmoteList.DraggingInfo obj)
	{
		draggingObject.SetActive(obj.isDragging && obj.slotIndex == index);
	}

	public override void Setup(string emoteName)
	{
		base.Setup(emoteName);
		if (currentEmoteInstance != null)
		{
			currentEmoteInstance.transform.localScale *= 0.575f;
		}
	}

	public void SetHighlight(bool value)
	{
		if (highlightObject != null)
		{
			highlightObject.SetActive(value);
		}
		if (nonHighlightObject != null)
		{
			nonHighlightObject.SetActive(!value);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetHighlight(value: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SetHighlight(value: false);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!string.IsNullOrEmpty(currentEmoteName))
		{
			SingletonBehaviour<UI_EmoteList>.instance.StartDragSlotItem(index);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		SingletonBehaviour<UI_EmoteList>.instance.DropSmart();
	}

	public void OnDrag(PointerEventData eventData)
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (DewInput.currentMode != InputMode.Gamepad)
		{
			return;
		}
		UI_EmoteList.DraggingInfo di = SingletonBehaviour<UI_EmoteList>.instance.draggingInfo;
		if (di.isDragging)
		{
			if (di.listIndex >= 0)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(SingletonBehaviour<UI_EmoteList>.instance.shownItems[di.listIndex].GetComponent<IGamepadFocusable>());
			}
			SingletonBehaviour<UI_EmoteList>.instance.DropOnSlot(index);
		}
		else if (!string.IsNullOrEmpty(currentEmoteName))
		{
			SingletonBehaviour<UI_EmoteList>.instance.StartDragSlotItem(index);
		}
	}
}
