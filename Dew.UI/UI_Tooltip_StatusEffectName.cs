public class UI_Tooltip_StatusEffectName : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is StatusEffect se))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		text.text = DewLocalization.GetUIValue(se.GetType().Name + "_Name");
	}
}
