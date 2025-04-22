public class Se_R_Composure_Buff : StatusEffect
{
	public ScalingValue critBonus;

	public float duration;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void EntityEventOnTakeDamage(EventInfoDamage obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
