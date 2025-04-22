public class Ai_Mon_Forest_BossDemon_MainSkill_SpawnMissiles_Missile : StandardProjectile
{
	public ScalingValue damage;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster);
		}
	}

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		DefaultDamage(damage).SetDirection(base.transform.forward).Dispatch(hit.entity);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
