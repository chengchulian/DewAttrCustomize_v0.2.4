public class UI_Tooltip_GemTitle : UI_Tooltip_BaseObj
{
	public bool useTemplate = true;

	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult data && currentObjects[1] is int playerIndex && currentObjects[2] is GemLocation gemLocation)
		{
			if (data.players[playerIndex].TryGetGemData(gemLocation, out var gemData))
			{
				string color0 = Dew.GetRarityColorHex(DewResources.GetByShortTypeName<Gem>(gemData.name).rarity);
				string template0 = DewLocalization.GetUIValue("InGame_Tooltip_GemName_" + Gem.GetQualityType(gemData.quality));
				if (!useTemplate)
				{
					template0 = "{0}";
				}
				string gemName0 = DewLocalization.GetGemName(DewLocalization.GetGemKey(gemData.name));
				text.text = "<color=" + color0 + ">" + string.Format(template0, gemName0) + "</color>";
			}
		}
		else if (base.currentObject is Gem gem)
		{
			string color1 = Dew.GetRarityColorHex(gem.rarity);
			int quality = gem.quality;
			if (currentObjects.Count == 2 && currentObjects[1] is int lvl)
			{
				quality = lvl;
			}
			else if (currentObjects.Count == 3 && currentObjects[1] is int lll && currentObjects[2] is string info && info == "RAW")
			{
				quality = lll;
			}
			string template1 = DewLocalization.GetUIValue("InGame_Tooltip_GemName_" + Gem.GetQualityType(quality));
			if (!useTemplate)
			{
				template1 = "{0}";
			}
			string gemName1 = DewLocalization.GetGemName(DewLocalization.GetGemKey(gem.GetType()));
			text.text = "<color=" + color1 + ">" + string.Format(template1, gemName1) + "</color>";
		}
	}
}
