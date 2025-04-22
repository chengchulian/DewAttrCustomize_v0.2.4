public class UI_Tooltip_RawText : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is string rawText))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		text.text = rawText;
	}
}
