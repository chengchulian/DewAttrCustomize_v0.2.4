using TMPro;
using UnityEngine;

public class UI_Tooltip_EquippedGemDescriber_Item : MonoBehaviour
{
	private const string LevelQualityDisplayTemplate = "<alpha=#AA><voffset=2><size=80%>{0}</size></voffset><alpha=#FF>";

	public string key = "";

	public void Setup(Gem gem, Hero hero)
	{
		string value = DewLocalization.GetUIValue(key);
		string rarityColor = Dew.GetRarityColorHex(gem.rarity);
		string gemKey = DewLocalization.GetGemKey(gem.GetType());
		string gemName = DewLocalization.GetGemName(gemKey);
		gemName += $"<alpha=#AA><voffset=2><size=80%>{$" {gem.quality:#,##0}%"}</size></voffset><alpha=#FF>";
		string head = "<b><color=" + rarityColor + ">[" + gemName + "]</color></b>";
		string desc = DewLocalization.ConvertDescriptionNodesToText(DewLocalization.GetGemDescription(gemKey), new DewLocalization.DescriptionSettings
		{
			contextEntity = hero,
			contextObject = gem
		});
		GetComponent<TextMeshProUGUI>().text = string.Format(value, head, desc);
	}

	public void Setup(DewGameResult.GemData gemData, DewGameResult.PlayerData playerData)
	{
		Gem gem = DewResources.GetByShortTypeName<Gem>(gemData.name);
		string value = DewLocalization.GetUIValue(key);
		string rarityColor = Dew.GetRarityColorHex(gem.rarity);
		string gemKey = DewLocalization.GetGemKey(gemData.name);
		string gemName = DewLocalization.GetGemName(gemKey);
		gemName += $"<alpha=#AA><voffset=2><size=80%>{$" {gemData.quality:#,##0}%"}</size></voffset><alpha=#FF>";
		string head = "<b><color=" + rarityColor + ">[" + gemName + "]</color></b>";
		string desc = DewLocalization.ConvertDescriptionNodesToText(DewLocalization.GetGemDescription(gemKey), new DewLocalization.DescriptionSettings
		{
			stats = new DewLocalization.EntityStats(playerData),
			currentLevel = gemData.quality,
			contextObject = gem,
			capturedFields = gemData.capturedTooltipFields
		});
		GetComponent<TextMeshProUGUI>().text = string.Format(value, head, desc);
	}
}
