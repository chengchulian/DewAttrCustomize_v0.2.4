using TMPro;

public class UI_Reveries_RemainingRerolls : LogicBehaviour
{
	private TextMeshProUGUI _text;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		_text.text = $"{DewSave.profile.remainingRerolls:###,0}/{3:###,0}";
	}
}
