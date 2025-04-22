using System;
using UnityEngine;

public class Se_Q_Suppression_Old : StatusEffect
{
	public bool resetAtk;

	public float duration;

	public float knockupAmount;

	public float stunDuration;

	public float hitRadius;

	public float mainTargetProcCoefficient;

	public float subTargetsProcCoefficient;

	public ScalingValue mainDamage;

	public ScalingValue subDamage;

	public GameObject mainTargetEffect;

	public GameObject subTargetEffect;

	[NonSerialized]
	public bool enableAreaStun;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		if (resetAtk)
		{
			base.victim.Ability.attackAbility.ResetCooldown();
		}
		DoAttackEmpower(delegate(EventInfoAttackEffect effect, int i)
		{
			if (effect.type != AttackEffectType.BasicAttackSub && !(effect.strength < 0.99f))
			{
				if (knockupAmount > 0f)
				{
					effect.victim.Visual.KnockUp(knockupAmount, isFriendly: false);
				}
				if (stunDuration > 0f)
				{
					CreateBasicEffect(effect.victim, new StunEffect(), stunDuration, "SuppressionStun");
				}
				ArrayReturnHandle<Entity> handle;
				ReadOnlySpan<Entity> readOnlySpan = DewPhysics.OverlapCircleAllEntities(out handle, effect.victim.agentPosition, hitRadius, tvDefaultHarmfulEffectTargets);
				for (int j = 0; j < readOnlySpan.Length; j++)
				{
					Entity entity = readOnlySpan[j];
					bool flag = entity == effect.victim;
					Damage(flag ? mainDamage : subDamage, flag ? mainTargetProcCoefficient : subTargetsProcCoefficient).SetElemental(ElementalType.Cold).SetOriginPosition(base.info.caster.agentPosition).Dispatch(entity, effect.chain.New(this));
					FxPlayNewNetworked(flag ? mainTargetEffect : subTargetEffect, entity);
					if (!flag && enableAreaStun)
					{
						if (knockupAmount > 0f)
						{
							entity.Visual.KnockUp(knockupAmount, isFriendly: false);
						}
						if (stunDuration > 0f)
						{
							CreateBasicEffect(entity, new StunEffect(), stunDuration, "SuppressionStun");
						}
					}
				}
				handle.Return();
			}
		}, 1, base.DestroyIfActive);
		SetTimer(duration);
	}

	private void MirrorProcessed()
	{
	}
}
