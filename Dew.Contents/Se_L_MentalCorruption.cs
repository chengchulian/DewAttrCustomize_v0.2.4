using UnityEngine;

public class Se_L_MentalCorruption : StatusEffect
{
	public ScalingValue tickDamageAmount;

	public float tickInterval = 1f;

	public float tickProcCoefficient = 0.5f;

	public GameObject damageEffect;

	private float _lastDamageTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			DestroyOnDeath(base.info.caster, includeKnockOuts: true);
			DestroyOnDeath(base.victim, includeKnockOuts: true);
			_lastDamageTime = Time.time;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && Time.time - _lastDamageTime > tickInterval)
		{
			_lastDamageTime += tickInterval;
			if (!base.info.caster.CheckEnemyOrNeutral(base.victim))
			{
				Destroy();
				return;
			}
			Damage(tickDamageAmount, tickProcCoefficient).SetElemental(ElementalType.Dark).Dispatch(base.victim);
			FxPlayNewNetworked(damageEffect, base.victim);
		}
	}

	private void MirrorProcessed()
	{
	}
}
