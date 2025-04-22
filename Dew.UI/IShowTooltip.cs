using UnityEngine.EventSystems;

public interface IShowTooltip : IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		if (SingletonBehaviour<UI_TooltipManager>.softInstance != null)
		{
			SingletonBehaviour<UI_TooltipManager>.softInstance.UpdateTooltip();
		}
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		if (SingletonBehaviour<UI_TooltipManager>.softInstance != null)
		{
			SingletonBehaviour<UI_TooltipManager>.softInstance.UpdateTooltip();
		}
	}

	void ShowTooltip(UI_TooltipManager tooltip);
}
