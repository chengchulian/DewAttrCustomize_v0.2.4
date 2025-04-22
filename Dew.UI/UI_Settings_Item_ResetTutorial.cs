public class UI_Settings_Item_ResetTutorial : UI_Settings_Item
{
	protected override void OnSetup()
	{
		GetComponentInChildren<UI_Toggle>().onIsCheckedChanged.AddListener(delegate(bool b)
		{
			base.parent._resetTutorial = b;
			base.parent.MarkAsDirty();
		});
	}

	public override void OnLoad()
	{
		GetComponentInChildren<UI_Toggle>().isChecked = false;
	}

	public override void LoadDefaults()
	{
		GetComponentInChildren<UI_Toggle>().isChecked = false;
	}
}
