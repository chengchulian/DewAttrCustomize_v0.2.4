public class UI_Tooltip_TreasureLore : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (base.currentObject is Treasure treasure)
		{
			text.text = DewLocalization.GetTreasureLore(DewLocalization.GetTreasureKey(treasure.GetType().Name));
		}
	}
}
