public class UI_EntityDreamDust : UI_AnimatedValueText
{
	private UI_EntityProvider _provider;

	protected override void Awake()
	{
		base.Awake();
		_provider = GetComponentInParent<UI_EntityProvider>();
	}

	protected override float GetValue()
	{
		if (_provider.target == null || _provider.target.owner == null)
		{
			return 0f;
		}
		return _provider.target.owner.dreamDust;
	}
}
