using System;
using UnityEngine;

public class Ai_R_ShadowWalk_Projectile : StandardProjectile
{
	public float procCoefficient = 1f;

	public ScalingValue damage;

	public float deviateMag = 3f;

	public float attackEffectStrength;

	private Vector3 _deviateVector;

	protected override void OnCreate()
	{
		_deviateVector = global::UnityEngine.Random.insideUnitSphere * deviateMag;
		base.OnCreate();
	}

	protected override Vector3 PositionSolver(float dt)
	{
		return base.PositionSolver(dt) + Mathf.Sin(base.normalizedPosition * MathF.PI) * _deviateVector;
	}

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		Damage(damage, procCoefficient).SetDirection(base.rotation).DoAttackEffect(AttackEffectType.Others, attackEffectStrength).SetElemental(ElementalType.Dark)
			.Dispatch(hit.entity);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
