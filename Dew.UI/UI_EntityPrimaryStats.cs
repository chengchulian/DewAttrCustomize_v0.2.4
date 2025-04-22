using TMPro;

public class UI_EntityPrimaryStats : LogicBehaviour
{
	public TextMeshProUGUI attackDamageText;

	public TextMeshProUGUI abilityPower;

	private UI_EntityProvider _provider;

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
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(target == null))
		{
			if (attackDamageText != null)
			{
				attackDamageText.text = target.Status.attackDamage.ToString("###,0");
			}
			if (abilityPower != null)
			{
				abilityPower.text = target.Status.abilityPower.ToString("###,0");
			}
		}
	}
}
