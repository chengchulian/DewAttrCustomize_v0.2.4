using System;

public class Ai_R_QuickTrigger_Projectile : StandardProjectile
{
	public ScalingValue dmg;

	public int maxHitCount = 3;

	[NonSerialized]
	public bool doAttackEffect;

	private int _currentHitCount;

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		DamageData damageData = Damage(dmg).SetDirection(base.info.rotation);
		if (doAttackEffect)
		{
			damageData.DoAttackEffect(AttackEffectType.Others);
		}
		damageData.Dispatch(hit.entity);
		_currentHitCount++;
		if (_currentHitCount >= maxHitCount)
		{
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
