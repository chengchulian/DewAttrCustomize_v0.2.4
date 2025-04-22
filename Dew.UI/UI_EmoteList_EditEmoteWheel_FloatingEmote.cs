using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EmoteList_EditEmoteWheel_FloatingEmote : UI_StationaryEmote
{
	public GameObject discardObject;

	private Vector3 _cv;

	private void Start()
	{
		SingletonBehaviour<UI_EmoteList>.instance.onDraggingInfoChanged += new Action<UI_EmoteList.DraggingInfo>(OnDraggingInfoChanged);
		discardObject.SetActive(value: false);
	}

	public override void Setup(string emoteName)
	{
		base.Setup(emoteName);
		if (currentEmoteInstance != null)
		{
			currentEmoteInstance.transform.localScale *= 0.7f;
		}
	}

	private void Update()
	{
		UI_EmoteList.DraggingInfo di = SingletonBehaviour<UI_EmoteList>.instance.draggingInfo;
		if (!di.isDragging)
		{
			discardObject.SetActive(value: false);
		}
		if (DewInput.currentMode == InputMode.KeyboardAndMouse)
		{
			_cv = default(Vector3);
			base.transform.position = Input.mousePosition;
		}
		else if (ManagerBase<GlobalUIManager>.instance.focused == null)
		{
			_cv = default(Vector3);
			SingletonBehaviour<UI_EmoteList>.instance.DropCancel();
		}
		else
		{
			base.transform.position = Vector3.SmoothDamp(base.transform.position, ManagerBase<GlobalUIManager>.instance.focused.GetTransform().position, ref _cv, 0.1f);
		}
		if (di.slotIndex < 0 || DewInput.currentMode == InputMode.Gamepad)
		{
			discardObject.SetActive(value: false);
		}
		else
		{
			if (!di.isDragging)
			{
				return;
			}
			bool isDiscard = true;
			ListReturnHandle<RaycastResult> handle;
			foreach (RaycastResult r in Dew.RaycastAllUIElementsBelowCursor(out handle))
			{
				if (r.gameObject.GetComponentInParent<UI_EmoteList_EditEmote>() != null)
				{
					isDiscard = false;
					break;
				}
				if (r.gameObject.GetComponentInParent<UI_EmoteList_EditEmoteWheel_Item>() != null)
				{
					isDiscard = false;
					break;
				}
			}
			handle.Return();
			discardObject.SetActive(isDiscard);
		}
	}

	private void OnDraggingInfoChanged(UI_EmoteList.DraggingInfo obj)
	{
		if (obj.isDragging)
		{
			if (obj.listIndex >= 0)
			{
				Setup(SingletonBehaviour<UI_EmoteList>.instance.shownItems[obj.listIndex].currentEmoteName);
			}
			else
			{
				Setup(SingletonBehaviour<UI_EmoteList_EditEmote>.instance.emotesState[obj.slotIndex]);
			}
		}
		else
		{
			Setup(null);
		}
	}
}
