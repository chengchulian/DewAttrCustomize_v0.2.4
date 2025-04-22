using System;
using UnityEngine;

public class Se_R_DangerousTheory : StatusEffect
{
	public GameObject fxActivateStrong;

	public float duration = 4f;

	public Knockback knockback;

	public ScalingValue damageAmount;

	public ScalingValue healAmount;

	public float bossAmp = 1f;

	public float lowAmp = 0.5f;

	public GameObject fxHit;

	public GameObject fxHeal;

	public float healHpThreshold = 0.5f;

	public float cooldownReductionRatio = 0.2f;

	private bool _isEmpowered;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			CreateAbilityInstance(base.position, null, new CastInfo(base.info.caster, base.info.point), delegate(Ai_M_FeatheryDash a)
			{
				a.speed *= 1.1f;
			});
			_isEmpowered = base.victim.currentHealth / base.victim.maxHealth - 0.02f < healHpThreshold;
			if (_isEmpowered && base.firstTrigger != null)
			{
				base.firstTrigger.ApplyCooldownReductionByRatio(cooldownReductionRatio);
			}
			if (_isEmpowered)
			{
				FxPlayNetworked(fxActivateStrong, base.victim);
			}
			base.victim.Ability.attackAbility.ResetCooldown();
			DoAttackCritical(null);
			base.victim.EntityEvent_OnAttackFiredBeforePrepare += new Action<EventInfoAttackFired>(EntityEventOnAttackFiredBeforePrepare);
			SetTimer(duration);
			ShowOnScreenTimer();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.EntityEvent_OnAttackFiredBeforePrepare -= new Action<EventInfoAttackFired>(EntityEventOnAttackFiredBeforePrepare);
		}
	}

	private void EntityEventOnAttackFiredBeforePrepare(EventInfoAttackFired obj)
	{
		if (!base.isActive)
		{
			return;
		}
		obj.instance.ActorEvent_OnAttackEffectTriggered += (Action<EventInfoAttackEffect>)delegate(EventInfoAttackEffect effect)
		{
			if (effect.type != AttackEffectType.Others)
			{
				knockback.ApplyWithOrigin(base.info.caster.position, effect.victim);
				DamageData damageData = Damage(damageAmount).ApplyStrength(effect.strength);
				HealData healData = Heal(GetValue(healAmount) * effect.strength);
				if (effect.victim.IsAnyBoss())
				{
					damageData.ApplyAmplification(bossAmp);
					damageData.SetAttr(DamageAttribute.IsCrit);
					healData.ApplyAmplification(bossAmp);
					healData.SetCrit();
					healData.SetCanMerge();
				}
				if (_isEmpowered)
				{
					damageData.ApplyAmplification(lowAmp);
					damageData.SetAttr(DamageAttribute.IsCrit);
				}
				damageData.Dispatch(effect.victim);
				if (_isEmpowered && !effect.victim.Status.hasDamageImmunity)
				{
					FxPlayNewNetworked(fxHeal, base.victim);
					healData.Dispatch(base.victim);
				}
				FxPlayNewNetworked(fxHit, effect.victim);
			}
		};
		DestroyIfActive();
	}

	private void MirrorProcessed()
	{
	}
}
