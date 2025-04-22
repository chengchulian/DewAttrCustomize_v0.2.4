using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby_SelectedHeroInfo_Mastery : UI_Lobby_SelectedHeroInfo_Base, IShowTooltip, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public TextMeshProUGUI levelText;

	public Image fillImage;

	protected override void OnHeroChanged()
	{
		base.OnHeroChanged();
		if (base.selectedHeroName != null && DewSave.profile.heroMasteries.TryGetValue(base.selectedHeroName, out var mastery))
		{
			levelText.text = mastery.currentLevel.ToString();
			fillImage.fillAmount = (float)mastery.currentPoints / (float)Dew.GetRequiredMasteryPointsToLevelUp(mastery.currentLevel);
		}
	}

	public void ShowTooltip(UI_TooltipManager tooltip)
	{
		if (DewSave.profile.heroMasteries.TryGetValue(base.selectedHeroName, out var mastery))
		{
			string current = mastery.currentPoints.ToString("#,##0");
			string max = Dew.GetRequiredMasteryPointsToLevelUp(mastery.currentLevel).ToString("#,##0");
			string remaining = (Dew.GetRequiredMasteryPointsToLevelUp(mastery.currentLevel) - mastery.currentPoints).ToString("#,##0");
			string total = mastery.totalPoints.ToString("#,##0");
			tooltip.ShowTitleDescTooltip((Func<Vector2>)(() => base.transform.position), DewLocalization.GetUIValue("TravelerMastery"), string.Format(DewLocalization.GetUIValue("TravelerMastery_Tooltip"), current, max, remaining, total));
		}
	}
}
