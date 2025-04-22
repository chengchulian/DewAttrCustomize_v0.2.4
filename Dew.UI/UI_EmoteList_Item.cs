using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EmoteList_Item : UI_StationaryEmote, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IShowTooltip, IPointerEnterHandler, IPointerExitHandler
{
	public Transform tooltipPivot;

	public GameObject lockedObject;

	public GameObject newObject;

	public GameObject draggingObject;

	public GameObject inUseObject;

	private UI_Toggle _toggle;

	private void Start()
	{
		_toggle = GetComponentInChildren<UI_Toggle>();
		draggingObject.SetActive(value: false);
		newObject.SetActive(DewSave.profile.emotes[currentEmoteName].isNew);
		lockedObject.SetActive(!DewSave.profile.emotes[currentEmoteName].isUnlocked);
		SingletonBehaviour<UI_EmoteList>.instance.onDraggingInfoChanged += new Action<UI_EmoteList.DraggingInfo>(OnDraggingInfoChanged);
		if (SingletonBehaviour<UI_EmoteList_EditEmote>.instance == null)
		{
			inUseObject.SetActive(value: false);
			return;
		}
		inUseObject.SetActive(SingletonBehaviour<UI_EmoteList_EditEmote>.instance.emotesState.Contains(currentEmoteName));
		SingletonBehaviour<UI_EmoteList_EditEmote>.instance.onItemsUpdated += (Action)delegate
		{
			if (!(this == null))
			{
				inUseObject.SetActive(SingletonBehaviour<UI_EmoteList_EditEmote>.instance.emotesState.Contains(currentEmoteName));
			}
		};
	}

	private void OnDraggingInfoChanged(UI_EmoteList.DraggingInfo obj)
	{
		draggingObject.SetActive(obj.isDragging && obj.listIndex == _toggle.index);
	}

	private void OnDestroy()
	{
		if (SingletonBehaviour<UI_EmoteList>.instance != null)
		{
			SingletonBehaviour<UI_EmoteList>.instance.onDraggingInfoChanged -= new Action<UI_EmoteList.DraggingInfo>(OnDraggingInfoChanged);
		}
	}

	public override void Setup(string emoteName)
	{
		base.Setup(emoteName);
		if (currentEmoteInstance != null)
		{
			currentEmoteInstance.transform.localScale *= 0.675f;
			((RectTransform)currentEmoteInstance.transform).anchoredPosition = Vector2.zero;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!(SingletonBehaviour<UI_EmoteList_EditEmote>.instance == null) && DewSave.profile.emotes[currentEmoteName].isUnlocked)
		{
			DewSave.profile.emotes[currentEmoteName].isNew = false;
			newObject.SetActive(value: false);
			SingletonBehaviour<UI_EmoteList>.instance.StartDragListItem(_toggle.index);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!(SingletonBehaviour<UI_EmoteList_EditEmote>.instance == null))
		{
			SingletonBehaviour<UI_EmoteList>.instance.DropSmart();
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		DewSave.profile.emotes[currentEmoteName].isNew = false;
		newObject.SetActive(value: false);
		SingletonBehaviour<UI_EmoteList>.instance.UpdateNewStatus();
		if (!(SingletonBehaviour<UI_EmoteList_EditEmote>.instance == null) && DewInput.currentMode == InputMode.Gamepad)
		{
			SingletonBehaviour<UI_EmoteList>.instance.StartDragListItem(_toggle.index);
			ManagerBase<GlobalUIManager>.instance.SetFocus(SingletonBehaviour<UI_EmoteList_EditEmote>.instance.wheelItems[0].GetComponent<IGamepadFocusable>());
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		Func<Vector2> posGetter = () => tooltipPivot.transform.position;
		if (DewSave.profile.emotes[currentEmoteName].isUnlocked)
		{
			tooltip.ShowRawTextTooltip(posGetter, DewLocalization.GetUIValue(currentEmoteName + "_Name"));
		}
		else
		{
			tooltip.ShowRawTextTooltip(posGetter, "???\n" + DewLocalization.GetUIValue("Collectables_YouHaveNotDiscoveredThisYet"));
		}
	}
}
