using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InGame_Result_HeroGemItem : MonoBehaviour, IGameResultStatItem, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public int index;

	public Image icon;

	public Transform tooltipPivot;

	[NonSerialized]
	public DewGameResult currentData;

	[NonSerialized]
	public int currentPlayerIndex;

	public UI_InGame_Result_HeroSkillItem parent => base.transform.parent.parent.GetComponentInChildren<UI_InGame_Result_HeroSkillItem>();

	public double UpdateAndGetScore(DewGameResult data, int playerIndex, float scoreMultiplier)
	{
		currentData = data;
		currentPlayerIndex = playerIndex;
		DewGameResult.PlayerData p = data.players[playerIndex];
		UI_InGame_Result_HeroSkillItem parent = this.parent;
		int gemIndex = p.gems.FindIndex((DewGameResult.GemData g) => g.location.skill == parent.type && g.location.index == index);
		if (gemIndex < 0)
		{
			icon.gameObject.SetActive(value: false);
			return 0.0;
		}
		icon.gameObject.SetActive(value: true);
		icon.sprite = DewResources.GetByShortTypeName<Gem>(p.gems[gemIndex].name).icon;
		return 0.0;
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		tooltip.ShowGemTooltip(tooltipPivot.position, currentData, currentPlayerIndex, new GemLocation(parent.type, index));
	}
}
