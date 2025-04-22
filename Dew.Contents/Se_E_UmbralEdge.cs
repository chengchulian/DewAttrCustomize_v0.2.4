using System;
using System.Collections;
using UnityEngine;

public class Se_E_UmbralEdge : StatusEffect
{
	public bool resetAttack;

	public float duration;

	public GameObject hitEffectOnVictim;

	public GameObject hitEffectOnCasterWhenMelee;

	public ScalingValue critChance;

	public ScalingValue hasteAmount;

	public ScalingValue hitDamage;

	public float procCoefficient;

	public float damageAmpForMelee;

	public bool addDurationOnKill;

	public ScalingValue addedDuration;

	public float hitDelay;

	public float startSlow;

	public float startSlowDuration;

	public bool startSlowDecay;

	private Hero _hero;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		_hero = (Hero)base.info.caster;
		_hero.ClientHeroEvent_OnKillOrAssist += new Action<EventInfoKill>(ClientHeroEventOnKillOrAssist);
		if (resetAttack)
		{
			_hero.Ability.attackAbility.ResetCooldown();
		}
		SetTimer(duration);
		ShowOnScreenTimer();
		DoAttackEmpower(delegate(EventInfoAttackEffect effect, int i)
		{
			if (!(_hero == null) && _hero.isActive && !effect.chain.DidReact(this))
			{
				_hero.StartCoroutine(Routine());
			}
			IEnumerator Routine()
			{
				FxPlayNewNetworked(hitEffectOnCasterWhenMelee, base.info.caster.position, Quaternion.LookRotation(effect.victim.position - base.info.caster.position).Flattened());
				float mult = 1f;
				if (_hero.IsMeleeHero())
				{
					mult *= 1f + damageAmpForMelee;
				}
				if (hitDelay > 0.0001f)
				{
					yield return new WaitForSeconds(hitDelay);
				}
				if (!(_hero == null) && _hero.isActive && !(this == null) && base.isActive)
				{
					FxPlayNewNetworked(hitEffectOnVictim, effect.victim);
					Damage(hitDamage, procCoefficient).ApplyStrength(effect.strength).ApplyRawMultiplier(mult).SetElemental(ElementalType.Dark)
						.SetOriginPosition(base.info.caster.position)
						.Dispatch(effect.victim, effect.chain.New(this));
				}
			}
		});
		DoStatBonus(new StatBonus
		{
			critChanceFlat = GetValue(critChance)
		});
		DoHaste(GetValue(hasteAmount));
		if (startSlow > 0.001f)
		{
			CreateBasicEffect(base.victim, new SlowEffect
			{
				decay = startSlowDecay,
				strength = startSlow
			}, startSlowDuration, "umbraledge_slow");
		}
	}

	private void ClientHeroEventOnKillOrAssist(EventInfoKill obj)
	{
		if (addDurationOnKill)
		{
			SetTimer(duration, Mathf.Clamp(base.remainingDuration.Value + GetValue(addedDuration), 0f, duration));
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && _hero != null)
		{
			_hero.ClientHeroEvent_OnKillOrAssist -= new Action<EventInfoKill>(ClientHeroEventOnKillOrAssist);
		}
	}

	private void MirrorProcessed()
	{
	}
}
