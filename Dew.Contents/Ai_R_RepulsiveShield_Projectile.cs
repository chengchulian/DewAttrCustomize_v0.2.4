public class Ai_R_RepulsiveShield_Projectile : StandardProjectile
{
	public ScalingValue damage;

	public Knockback knockback;

	public float procCoefficient = 0.5f;

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		Damage(damage, procCoefficient).SetDirection(base.rotation).Dispatch(hit.entity);
		if (hit.entity.Control.isDashing)
		{
			knockback.ApplyWithDirection(base.info.caster.transform.forward, hit.entity);
		}
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
