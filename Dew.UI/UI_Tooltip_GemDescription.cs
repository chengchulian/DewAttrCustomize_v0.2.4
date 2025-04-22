using System.Collections.Generic;
using DewInternal;

public class UI_Tooltip_GemDescription : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!DoResultTooltip())
		{
			DoInGameTooltip();
		}
	}

	private bool DoResultTooltip()
	{
		if (currentObjects.Count != 3 || !(currentObjects[0] is DewGameResult data) || !(currentObjects[1] is int playerIndex) || !(currentObjects[2] is GemLocation gemLocation))
		{
			return false;
		}
		if (!data.players[playerIndex].TryGetGemData(gemLocation, out var gemData))
		{
			return true;
		}
		IReadOnlyList<LocaleNode> nodes0 = DewLocalization.GetGemDescription(DewLocalization.GetGemKey(gemData.name));
		text.text = DewLocalization.ConvertDescriptionNodesToText(nodes0, new DewLocalization.DescriptionSettings
		{
			contextObject = DewResources.GetByShortTypeName<Gem>(gemData.name),
			currentLevel = gemData.quality,
			stats = new DewLocalization.EntityStats(data.players[playerIndex]),
			capturedFields = gemData.capturedTooltipFields
		});
		return true;
	}

	private void DoInGameTooltip()
	{
		if (base.currentObject is Gem gem)
		{
			IReadOnlyList<LocaleNode> nodes = DewLocalization.GetGemDescription(DewLocalization.GetGemKey(gem.GetType()));
			if (currentObjects.Count == 3 && currentObjects[1] is int fromLevel && currentObjects[2] is int toLevel)
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes, new DewLocalization.DescriptionSettings
				{
					contextEntity = ((DewPlayer.local == null) ? null : DewPlayer.local.hero),
					contextObject = gem,
					currentLevel = toLevel,
					previousLevel = fromLevel,
					showLevelScaling = true
				});
			}
			else if (currentObjects.Count == 2 && currentObjects[1] is int level)
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes, new DewLocalization.DescriptionSettings
				{
					contextEntity = ((DewPlayer.local == null) ? null : DewPlayer.local.hero),
					contextObject = gem,
					currentLevel = level
				});
			}
			else if (currentObjects.Count == 2 && currentObjects[1] is Hero h)
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes, new DewLocalization.DescriptionSettings
				{
					contextEntity = h,
					contextObject = gem
				});
			}
			else if (currentObjects.Count == 3 && currentObjects[1] is int lll && currentObjects[2] is string info && info == "RAW")
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes, new DewLocalization.DescriptionSettings
				{
					contextObject = gem,
					currentLevel = lll
				});
			}
			else
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes, new DewLocalization.DescriptionSettings
				{
					contextEntity = ((DewPlayer.local == null) ? null : DewPlayer.local.hero),
					contextObject = gem
				});
			}
		}
	}
}
