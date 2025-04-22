using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EntityManaBar : UI_EntityBar, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	protected override float GetFillAmount()
	{
		if (base.target == null)
		{
			return 0f;
		}
		return base.target.currentMana / base.target.maxMana;
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		string manaKey = base.target.Status.manaTypeKey;
		tooltip.ShowTitleDescTooltip(Input.mousePosition, DewLocalization.GetUIValue("InGame_Tooltip_Mana_" + manaKey + "_Title"), DewLocalization.GetUIValue("InGame_Tooltip_Mana_" + manaKey + "_Description"));
	}
}
