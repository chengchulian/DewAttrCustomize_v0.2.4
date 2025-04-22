public class UI_Tooltip_LocalizationText : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is string key) || key == "")
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		text.text = DewLocalization.GetUIValue(key);
	}
}
