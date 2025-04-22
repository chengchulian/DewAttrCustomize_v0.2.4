using System;
using System.Collections;
using UnityEngine;

public class Se_R_UnbreakableDetermination : StatusEffect
{
	public float initDuration;

	public float explodeDelay = 0.3f;

	public float damageDelay = 0.1f;

	public float invulnerableDuration;

	public float speedDuration;

	public float speedAmount = 100f;

	public TriggerChannelData startChannelData;

	public float endDazeDuration;

	public GameObject explodeEffect;

	public GameObject explosionHit;

	public ScalingValue damage;

	public DewCollider range;

	public ScalingValue hasteAmount;

	public ScalingValue skillHasteAmount;

	public float dmgAmp;

	public float addedDuration;

	public float healLostHealthRatio;

	public float healMinAmount;

	public GameObject healEffect;

	public GameObject prepareEffect;

	private StatBonus _givenBonus;

	private Se_D_AstridsMasterpiece _empoweredStatusEffect;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		DoHaste(GetValue(hasteAmount));
		_givenBonus = new StatBonus
		{
			abilityHasteFlat = GetValue(skillHasteAmount)
		};
		base.victim.Status.AddStatBonus(_givenBonus);
		SetTimer(initDuration + explodeDelay);
		ShowOnScreenTimer();
		if (base.victim.Status.TryGetStatusEffect<Se_D_AstridsMasterpiece>(out var effect))
		{
			foreach (Actor child in effect.children)
			{
				if (child is Se_D_AstridsMasterpiece_Exposed se_D_AstridsMasterpiece_Exposed)
				{
					se_D_AstridsMasterpiece_Exposed.chargeCount = se_D_AstridsMasterpiece_Exposed.maxCharge;
				}
			}
			_empoweredStatusEffect = effect;
			_empoweredStatusEffect.ActorEvent_OnDealDamage += new Action<EventInfoDamage>(AstridHeal);
		}
		SkillTrigger movement = ((Hero)base.victim).Skill.Movement;
		movement.SetCharge(0, movement.configs[0].maxCharges);
		base.victim.dealtDamageProcessor.Add(AmpDamage);
		base.victim.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(AttackEffect);
		CreateBasicEffect(base.victim, new UncollidableEffect(), invulnerableDuration, "mist_r_uncol");
		CreateBasicEffect(base.victim, new InvulnerableEffect(), invulnerableDuration, "mist_r_invul");
		CreateBasicEffect(base.victim, new SpeedEffect
		{
			strength = speedAmount
		}, speedDuration, "mist_r_speed");
		base.victim.Control.StartChannel(startChannelData.CreateChannel(Explode, Explode, new AbilitySelfValidator()));
		base.victim.Control.StartChannel(new Channel
		{
			blockedActions = (Channel.BlockedAction.Ability | Channel.BlockedAction.Attack),
			duration = startChannelData.duration + endDazeDuration
		});
		FxPlayNetworked(prepareEffect, base.victim);
	}

	private void Explode()
	{
		StartSequence(ExplodeSequence());
	}

	private IEnumerator ExplodeSequence()
	{
		yield return new SI.WaitForSeconds(explodeDelay);
		FxStopNetworked(prepareEffect);
		FxPlayNewNetworked(explodeEffect, base.victim);
		yield return new SI.WaitForSeconds(damageDelay);
		ArrayReturnHandle<Entity> handle;
		ReadOnlySpan<Entity> entities = range.GetEntities(out handle, tvDefaultHarmfulEffectTargets);
		for (int i = 0; i < entities.Length; i++)
		{
			Entity entity = entities[i];
			Damage(damage).SetOriginPosition(base.victim.position).Dispatch(entity);
			FxPlayNewNetworked(explosionHit, entity);
		}
		handle.Return();
	}

	private void AstridHeal(EventInfoDamage obj)
	{
		FxPlayNewNetworked(healEffect, base.victim);
		float originalAmount = Mathf.Max(healMinAmount, base.victim.Status.missingHealth * healLostHealthRatio);
		DoHeal(new HealData(originalAmount), base.victim);
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.Status.RemoveStatBonus(_givenBonus);
			base.victim.dealtDamageProcessor.Remove(AmpDamage);
			base.victim.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(AttackEffect);
			if (_empoweredStatusEffect != null)
			{
				_empoweredStatusEffect.ActorEvent_OnDealDamage -= new Action<EventInfoDamage>(AstridHeal);
			}
		}
	}

	private void AmpDamage(ref DamageData data, Actor actor, Entity target)
	{
		if (!data.IsAmountModifiedBy(this) && base.victim.CheckEnemyOrNeutral(target))
		{
			data.ApplyAmplification(dmgAmp);
			data.SetAmountModifiedBy(this);
		}
	}

	private void AttackEffect(EventInfoAttackEffect obj)
	{
		SetTimer(initDuration, Mathf.Min(base.remainingDuration.Value + addedDuration * obj.strength, initDuration));
		CreateAbilityInstance(obj.victim.position, null, new CastInfo(base.info.caster), delegate(Ai_R_UnbreakableDetermination_SubExplosion s)
		{
			s.strengthMultiplier = obj.strength;
		});
	}

	private void MirrorProcessed()
	{
	}
}
