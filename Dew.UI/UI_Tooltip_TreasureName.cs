public class UI_Tooltip_TreasureName : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (base.currentObject is Treasure treasure)
		{
			text.text = DewLocalization.GetTreasureName(DewLocalization.GetTreasureKey(treasure.GetType().Name));
		}
	}
}
