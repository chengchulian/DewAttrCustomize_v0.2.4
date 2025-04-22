public class Se_E_Rewind_Damage : StatusEffect
{
	public ScalingValue damage;

	public float damageDelay;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			SetTimer(damageDelay);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && !(base.victim == null))
		{
			Damage(damage).Dispatch(base.victim, chain);
		}
	}

	private void MirrorProcessed()
	{
	}
}
