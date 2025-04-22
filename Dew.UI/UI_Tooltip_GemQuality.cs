public class UI_Tooltip_GemQuality : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		string prefix = DewLocalization.GetUIValue("InGame_Tooltip_GemQuality");
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult data && currentObjects[1] is int playerIndex && currentObjects[2] is GemLocation gemLocation)
		{
			if (data.players[playerIndex].TryGetGemData(gemLocation, out var gemData))
			{
				text.text = $"{prefix} {gemData.quality:###,0}%";
			}
		}
		else if (base.currentObject is Gem gem)
		{
			if (currentObjects.Count == 3 && currentObjects[1] is int fromLevel && currentObjects[2] is int toLevel)
			{
				text.text = prefix + " " + DewLocalization.RenderChangedValue($"{fromLevel:###,0}%", $"{toLevel:###,0}%");
			}
			else if (currentObjects.Count == 2 && currentObjects[1] is int level)
			{
				text.text = $"{prefix} {level:###,0}%";
			}
			else if (currentObjects.Count == 3 && currentObjects[1] is int lll && currentObjects[2] is string info && info == "RAW")
			{
				text.text = $"{prefix} {lll:###,0}%";
			}
			else if (gem.quality == -1)
			{
				text.text = prefix + " 100%";
			}
			else
			{
				text.text = $"{prefix} {gem.quality:###,0}%";
			}
		}
	}
}
