using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Lobby_Constellations_AlphaStar : MonoBehaviour, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Transform tooltipPivot;

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		tooltip.ShowRawTextTooltip(tooltipPivot.position, string.Format(DewLocalization.GetUIValue("Constellations_AlphaStarTemplate"), DewLocalization.GetUIValue(DewPlayer.local.selectedHeroType + "_Name")));
	}

	private void OnDisable()
	{
		if (SingletonBehaviour<UI_TooltipManager>.instance != null)
		{
			SingletonBehaviour<UI_TooltipManager>.instance.UpdateTooltip();
		}
	}
}
