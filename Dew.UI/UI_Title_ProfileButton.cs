using TMPro;

public class UI_Title_ProfileButton : LogicBehaviour
{
	public TextMeshProUGUI text;

	private void Start()
	{
		text.text = DewLocalization.GetUIValue("Title_Profile_Profile") + ": " + ((DewSave.profile == null) ? "" : DewSave.profile.name);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		text.text = DewLocalization.GetUIValue("Title_Profile_Profile") + ": " + ((DewSave.profile == null) ? "" : DewSave.profile.name);
	}
}
