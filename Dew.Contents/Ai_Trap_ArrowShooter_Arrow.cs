public class Ai_Trap_ArrowShooter_Arrow : StandardProjectile
{
	public float startHeight;

	public float dmgMaxHpRatio;

	public float monsterDmgMultiplier;

	public Knockback knockback;

	public float stunDuration;

	protected override void OnPrepare()
	{
	}

	protected override void OnEntity(EntityHit hit)
	{
	}

	private void MirrorProcessed()
	{
	}
}
