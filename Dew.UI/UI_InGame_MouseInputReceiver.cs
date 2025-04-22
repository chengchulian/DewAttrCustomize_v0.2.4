using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InGame_MouseInputReceiver : SingletonBehaviour<UI_InGame_MouseInputReceiver>, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IShowTooltip, IPointerEnterHandler, IPointerExitHandler
{
	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		if (ManagerBase<FloatingWindowManager>.instance != null && ManagerBase<FloatingWindowManager>.instance.currentTarget != null)
		{
			ManagerBase<FloatingWindowManager>.instance.ClearTarget();
		}
		if (NetworkedManagerBase<ConsoleManager>.instance != null)
		{
			NetworkedManagerBase<ConsoleManager>.instance.OnGameAreaPointerDown(eventData);
		}
		if (ManagerBase<ControlManager>.instance != null)
		{
			InGameUIManager.instance.isWorldDisplayed = WorldDisplayStatus.None;
		}
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (!(ManagerBase<FloatingWindowManager>.instance.currentTarget != null) && ManagerBase<ControlManager>.instance.focusedInteractable != null && ManagerBase<ControlManager>.instance.isFocusedInteractableAtCursor && DewLocalization.TryGetUIValue(ManagerBase<ControlManager>.instance.focusedInteractable.GetType().Name + "_Description", out var val))
		{
			IInteractable interactable = ManagerBase<ControlManager>.instance.focusedInteractable;
			tooltip.ShowRawTextTooltip(new TooltipSettings
			{
				mode = TooltipPositionMode.Getter,
				getter = () => (interactable.IsUnityNull() || ManagerBase<DewCamera>.instance == null) ? (Vector2.left * 10000f) : ((Vector2)ManagerBase<DewCamera>.softInstance.mainCamera.WorldToScreenPoint(interactable.interactPivot.position + Vector3.up)),
				customDelay = 0.5f
			}, val);
		}
	}
}
