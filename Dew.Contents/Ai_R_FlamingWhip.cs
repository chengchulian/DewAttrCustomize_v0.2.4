public class Ai_R_FlamingWhip : InstantDamageInstance
{
	public float slowAmount;

	public float slowDuration;

	protected override void OnHit(Entity entity)
	{
		base.OnHit(entity);
		CreateBasicEffect(entity, new SlowEffect
		{
			strength = slowAmount
		}, slowDuration, "flamingwhip_slow");
	}

	private void MirrorProcessed()
	{
	}
}
