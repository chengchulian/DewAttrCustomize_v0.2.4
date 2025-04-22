using System;
using UnityEngine;

public class Se_Q_IncendiaryRounds_EmpowerAttacks : StatusEffect
{
	public float duration;

	public int numOfAttacks = 4;

	public ScalingValue hasteAmount;

	public bool speedOnShoot;

	public bool isSpeedDecay;

	public float speedDuration;

	public ScalingValue speedAmount;

	private int _doneAttacks;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			if (base.victim.Ability.attackAbility != null)
			{
				base.victim.Ability.attackAbility.ResetCooldown();
			}
			DoHaste(GetValue(hasteAmount));
			base.victim.EntityEvent_OnAttackFired += new Action<EventInfoAttackFired>(EntityEventOnAttackFired);
			SetTimer(duration);
			ShowOnScreenTimer();
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.EntityEvent_OnAttackFired -= new Action<EventInfoAttackFired>(EntityEventOnAttackFired);
		}
	}

	private void EntityEventOnAttackFired(EventInfoAttackFired obj)
	{
		_doneAttacks++;
		ResetTimer();
		CreateAbilityInstance(base.transform.position, Quaternion.identity, new CastInfo(base.victim), delegate(Ai_Q_IncendiaryRounds_Attack r)
		{
			r.NetworkattackInstance = obj.instance;
		});
		if (speedOnShoot)
		{
			CreateBasicEffect(base.victim, new SpeedEffect
			{
				decay = isSpeedDecay,
				strength = GetValue(speedAmount)
			}, speedDuration, "incendiaryrounds_speed", DuplicateEffectBehavior.UsePrevious);
		}
		if (_doneAttacks >= numOfAttacks)
		{
			Destroy();
		}
	}

	private void MirrorProcessed()
	{
	}
}
