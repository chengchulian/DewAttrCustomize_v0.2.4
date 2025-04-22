using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace viperOSK;

public class OSK_UI_CustomReceiver : OSK_Receiver, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, ISelectHandler, ISubmitHandler
{
	public UnityEvent onSelect;

	public UnityEvent onSelectClick;

	private void Awake()
	{
		if (base.gameObject.TryGetComponent<TMP_Text>(out textReceiver))
		{
			textReceiver.color = normalColor;
			cursor = base.transform.GetComponentInChildren<I_OSK_Cursor>();
			if (cursor == null)
			{
				Debug.LogWarning("This TMPro text object does not have a cursor. If you want full edit functionality add a OSK_UI_Cursor prefab as a child");
			}
			OnFocusLost();
		}
		else
		{
			Debug.LogWarning("The OSK_UI_CustomReceiver must be in the same gameobject as the TextMeshPro text object receiving text input");
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (cursor != null && allowTextSelection)
		{
			Deselect();
			cursorSel.x = Selection(eventData.pointerPressRaycast.screenPosition);
			cursorSel.x = Mathf.Min(cursorSel.x, textReceiver.text.Length - 1);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!isFocused() && onSelectClick != null)
		{
			onSelectClick.Invoke();
			OnFocus();
		}
		else
		{
			if (cursor == null)
			{
				return;
			}
			if (cursorSel.x >= 0 && allowTextSelection)
			{
				cursorSel.y = Selection(eventData.pointerCurrentRaycast.screenPosition, charhit: true);
				if (cursorSel.y < 0)
				{
					cursorSel.y = Selection(eventData.pointerCurrentRaycast.screenPosition);
				}
				cursorIndex = cursorSel.y + ((cursorSel.x < cursorSel.y) ? 1 : 0);
				cursorSel.y = Mathf.Min(cursorSel.y, textReceiver.text.Length - 1);
				if (cursorSel.x == cursorSel.y)
				{
					ref Vector2Int reference = ref cursorSel;
					int x = (cursorSel.y = -1);
					reference.x = x;
				}
				else
				{
					int temp = cursorSel.x;
					cursorSel.x = Mathf.Min(cursorSel.x, cursorSel.y);
					cursorSel.y = Mathf.Max(temp, cursorSel.y);
					SelectionHighlight(highlightColor);
				}
			}
			else
			{
				Deselect();
				cursorIndex = Selection(eventData.pointerCurrentRaycast.screenPosition);
			}
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		if (onSelect != null)
		{
			onSelect.Invoke();
		}
	}

	void ISubmitHandler.OnSubmit(BaseEventData eventData)
	{
		if (onSelectClick != null)
		{
			onSelectClick.Invoke();
		}
		if (!isFocused())
		{
			OnFocus();
		}
	}

	public override int Selection(Vector3 hitpoint, bool charhit = false)
	{
		Camera cam = Camera.main;
		if (textReceiver.canvas != null)
		{
			cam = ((textReceiver.canvas.renderMode != 0) ? textReceiver.canvas.worldCamera : null);
		}
		if (charhit)
		{
			return TMP_TextUtilities.FindIntersectingCharacter(textReceiver, hitpoint, cam, visibleOnly: false);
		}
		return TMP_TextUtilities.GetCursorIndexFromPosition(textReceiver, hitpoint, cam);
	}

	public override void Deselect()
	{
		SelectionHighlight(normalColor, all: true);
		ref Vector2Int reference = ref cursorSel;
		int x = (cursorSel.y = -1);
		reference.x = x;
	}

	public override void SelectionHighlight(Color32 c, bool all = false)
	{
		if (selection != 0)
		{
			_ = textReceiver.textInfo;
			int start = cursorSel.x;
			int end = cursorSel.y;
			if (all)
			{
				start = 0;
				end = textReceiver.text.Length - 1;
			}
			for (int i = start; i <= end; i++)
			{
				int meshIndex = textReceiver.textInfo.characterInfo[i].materialReferenceIndex;
				int vertexIndex = textReceiver.textInfo.characterInfo[i].vertexIndex;
				Color32[] colors = textReceiver.textInfo.meshInfo[meshIndex].colors32;
				colors[vertexIndex] = c;
				colors[vertexIndex + 1] = c;
				colors[vertexIndex + 2] = c;
				colors[vertexIndex + 3] = c;
			}
			textReceiver.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}
	}

	private void Update()
	{
	}
}
