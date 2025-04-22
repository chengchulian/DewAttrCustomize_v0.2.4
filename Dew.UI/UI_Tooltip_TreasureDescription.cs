using System.Collections.Generic;
using DewInternal;

public class UI_Tooltip_TreasureDescription : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (base.currentObject is Treasure treasure)
		{
			IReadOnlyList<LocaleNode> nodes = DewLocalization.GetTreasureDescription(DewLocalization.GetTreasureKey(treasure.GetType().Name));
			text.text = DewLocalization.ConvertDescriptionNodesToText(nodes, new DewLocalization.DescriptionSettings
			{
				contextEntity = ((DewPlayer.local == null) ? null : DewPlayer.local.hero),
				contextObject = treasure
			});
		}
	}
}
