using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TitleDescTooltip : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public bool useCustomTooltipPos;

	public Vector2 customTooltipOffset;

	public string titleKey;

	public string descKey;

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (useCustomTooltipPos)
		{
			tooltip.ShowTitleDescTooltip(base.transform.position + (Vector3)customTooltipOffset, DewLocalization.GetUIValue(titleKey), DewLocalization.GetUIValue(descKey));
		}
		else
		{
			tooltip.ShowTitleDescTooltip(null, DewLocalization.GetUIValue(titleKey), DewLocalization.GetUIValue(descKey));
		}
	}

	private void OnDisable()
	{
		if (SingletonBehaviour<UI_TooltipManager>.instance != null)
		{
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		}
	}
}
