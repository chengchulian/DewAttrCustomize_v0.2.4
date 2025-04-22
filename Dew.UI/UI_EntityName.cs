using TMPro;

public class UI_EntityName : LogicBehaviour
{
	private UI_EntityProvider _provider;

	private TextMeshProUGUI _text;

	public Entity target
	{
		get
		{
			if (!(_provider != null))
			{
				return null;
			}
			return _provider.target;
		}
	}

	protected virtual void Awake()
	{
		_provider = GetComponentInParent<UI_EntityProvider>();
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		if (target == null)
		{
			return;
		}
		if (target is Hero hero && hero.owner != null)
		{
			if (hero.owner != null && hero.owner.isHumanPlayer)
			{
				_text.text = hero.owner.playerName;
			}
			else
			{
				_text.text = DewLocalization.GetUIValue(hero.GetType().Name + "_Name");
			}
		}
		else
		{
			_text.text = DewLocalization.GetUIValue(target.GetType().Name + "_Name");
		}
	}
}
