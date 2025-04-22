public class Ai_R_Scattershot_Projectile : StandardProjectile
{
	public ScalingValue damage;

	public float attackEffectStrength = 0.5f;

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		Damage(damage).SetDirection(base.rotation).DoAttackEffect(AttackEffectType.Others, attackEffectStrength).SetElemental(ElementalType.Dark)
			.SetAttr(DamageAttribute.ForceMergeNumber)
			.Dispatch(hit.entity);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
