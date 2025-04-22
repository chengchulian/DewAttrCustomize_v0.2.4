using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IngameDebugConsole;

public class DebugsOnScrollListener : MonoBehaviour, IScrollHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
	public ScrollRect debugsScrollRect;

	public DebugLogManager debugLogManager;

	public void OnScroll(PointerEventData data)
	{
		debugLogManager.SnapToBottom = IsScrollbarAtBottom();
	}

	public void OnBeginDrag(PointerEventData data)
	{
		debugLogManager.SnapToBottom = false;
	}

	public void OnEndDrag(PointerEventData data)
	{
		debugLogManager.SnapToBottom = IsScrollbarAtBottom();
	}

	public void OnScrollbarDragStart(BaseEventData data)
	{
		debugLogManager.SnapToBottom = false;
	}

	public void OnScrollbarDragEnd(BaseEventData data)
	{
		debugLogManager.SnapToBottom = IsScrollbarAtBottom();
	}

	private bool IsScrollbarAtBottom()
	{
		return debugsScrollRect.verticalNormalizedPosition <= 1E-06f;
	}
}
