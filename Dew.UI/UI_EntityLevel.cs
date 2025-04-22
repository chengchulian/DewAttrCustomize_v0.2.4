using TMPro;

public class UI_EntityLevel : LogicBehaviour
{
	public TextMeshProUGUI text;

	private UI_EntityProvider _provider;

	private void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(_provider.target == null))
		{
			text.text = _provider.target.level.ToString();
		}
	}
}
