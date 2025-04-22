public class Ai_R_BlackArbalest : StandardProjectile
{
	public ScalingValue hitDamage;

	public float ampPerHit;

	public bool doSelfKnockback;

	public float attackEffectStrength = 1f;

	public Dash selfKnockback;

	public bool doTargetKnockback;

	public Knockback targetKnockback;

	private int _prevHitCount;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer && doSelfKnockback)
		{
			selfKnockback.ApplyByDirection(base.info.caster, -base.info.forward);
		}
	}

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		DamageData damageData = Damage(hitDamage).SetDirection(base.rotation).DoAttackEffect(AttackEffectType.Others, attackEffectStrength).SetElemental(ElementalType.Dark);
		if (_prevHitCount >= 2)
		{
			damageData.SetAttr(DamageAttribute.IsCrit);
		}
		damageData.ApplyAmplification(ampPerHit * (float)_prevHitCount);
		damageData.Dispatch(hit.entity);
		if (doTargetKnockback)
		{
			targetKnockback.ApplyWithDirection(base.rotation, hit.entity);
		}
		_prevHitCount++;
	}

	private void MirrorProcessed()
	{
	}
}
