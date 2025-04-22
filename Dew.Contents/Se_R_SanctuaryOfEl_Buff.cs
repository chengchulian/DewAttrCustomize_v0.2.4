public class Se_R_SanctuaryOfEl_Buff : StatusEffect
{
	public float duration;

	public ScalingValue hasteAmount;

	public bool doInvulnerable;

	public ScalingValue armorAmount;

	public float selfMultiplier;

	private OnScreenTimerHandle _handle;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.victim.isOwned && base.info.caster != null && !base.info.caster.isOwned)
		{
			Ai_R_SanctuaryOfEl_Ground ground = FindFirstAncestorOfType<Ai_R_SanctuaryOfEl_Ground>();
			_handle = ShowOnScreenTimerLocally(new OnScreenTimerHandle
			{
				fillAmountGetter = () => (!(ground != null)) ? 0f : ground.fillAmount
			});
		}
		if (base.isServer)
		{
			if (doInvulnerable)
			{
				DoInvulnerable();
			}
			if (base.victim == base.info.caster)
			{
				DoUnstoppable();
			}
			float num = GetValue(armorAmount) * ((base.victim == base.info.caster) ? selfMultiplier : 1f);
			if (num > 0f)
			{
				DoArmorBoost(num);
			}
			DoHaste(GetValue(hasteAmount) * ((base.victim == base.info.caster) ? selfMultiplier : 1f));
			SetTimer(duration);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (_handle != null)
		{
			HideOnScreenTimerLocally(_handle);
			_handle = null;
		}
	}

	private void MirrorProcessed()
	{
	}
}
