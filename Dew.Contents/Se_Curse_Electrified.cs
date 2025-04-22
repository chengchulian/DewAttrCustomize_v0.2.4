using System;
using UnityEngine;

public class Se_Curse_Electrified : CurseStatusEffect
{
	public GameObject stunEffect;

	public float[] stunDurations;

	public float cooldownTime;

	private float _lastStunTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.EntityEvent_OnTakeDamage += new Action<EventInfoDamage>(CheckStun);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.EntityEvent_OnTakeDamage -= new Action<EventInfoDamage>(CheckStun);
		}
	}

	private void CheckStun(EventInfoDamage obj)
	{
		if (!(Time.time - _lastStunTime < cooldownTime))
		{
			_lastStunTime = Time.time;
			CreateBasicEffect(base.victim, new StunEffect(), GetValue(stunDurations), "hatred_electrify");
			FxPlayNewNetworked(stunEffect, base.victim);
		}
	}

	private void MirrorProcessed()
	{
	}
}
