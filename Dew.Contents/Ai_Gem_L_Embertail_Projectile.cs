using System;
using UnityEngine;

public class Ai_Gem_L_Embertail_Projectile : StandardProjectile
{
	public ScalingValue damage;

	public float procCoefficient = 0.5f;

	public float deviateMag = 4f;

	internal float _strengthMultiplier;

	private Vector3 _deviateVector;

	protected override void OnCreate()
	{
		_deviateVector = global::UnityEngine.Random.insideUnitSphere * deviateMag;
		if (_deviateVector.y < 0f)
		{
			_deviateVector.y *= -1f;
		}
		base.OnCreate();
	}

	protected override Vector3 PositionSolver(float dt)
	{
		return base.PositionSolver(dt) + Mathf.Sin(base.normalizedPosition * MathF.PI) * _deviateVector;
	}

	protected override void OnEntity(EntityHit hit)
	{
		base.OnEntity(hit);
		Damage(damage, procCoefficient * _strengthMultiplier).SetElemental(ElementalType.Fire).SetDirection(base.rotation).ApplyRawMultiplier(_strengthMultiplier)
			.Dispatch(hit.entity, chain);
		Destroy();
	}

	private void MirrorProcessed()
	{
	}
}
