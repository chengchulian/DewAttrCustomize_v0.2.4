using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Reveries_ItemsGroup : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		UI_Reveries_SlotBase parent = GetComponentInParent<UI_Reveries_SlotBase>();
		tooltip.ShowItemsTooltip((Func<Vector2>)(() => base.transform.position), parent.data.grantedItems);
	}
}
