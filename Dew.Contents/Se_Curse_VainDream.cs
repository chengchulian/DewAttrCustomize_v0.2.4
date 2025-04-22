using System;
using UnityEngine;

public class Se_Curse_VainDream : CurseStatusEffect
{
	public GameObject fxExplode;

	public float[] lostRatio;

	public float cooldownTime;

	private float _lastLostTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			base.victim.EntityEvent_OnTakeDamage += new Action<EventInfoDamage>(CheckLose);
		}
	}

	protected override void OnDestroyActor()
	{
		base.OnDestroyActor();
		if (base.isServer && base.victim != null)
		{
			base.victim.EntityEvent_OnTakeDamage -= new Action<EventInfoDamage>(CheckLose);
		}
	}

	private void CheckLose(EventInfoDamage obj)
	{
		if (base.victim.owner == null || obj.actor == null)
		{
			return;
		}
		Entity entity = obj.actor.firstEntity;
		if (!(entity == null) && entity.CheckEnemyOrNeutral(base.victim) && !(Time.time - _lastLostTime < cooldownTime))
		{
			int num = Mathf.RoundToInt((float)base.victim.owner.dreamDust * GetValue(lostRatio));
			if (num > 0)
			{
				_lastLostTime = Time.time;
				FxPlayNewNetworked(fxExplode, base.victim);
				base.victim.owner.dreamDust -= num;
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
