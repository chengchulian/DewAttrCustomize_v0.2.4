using System;

public class Ai_R_SerpentineBlessing_Projectile : StandardProjectile
{
	public ScalingValue damage;

	public ScalingValue healMaxHpRatio;

	public float stunDuration;

	[NonSerialized]
	public float strength;

	[NonSerialized]
	public Entity healTarget;

	protected override void OnEntity(EntityHit hit)
	{
	}

	private void MirrorProcessed()
	{
	}
}
