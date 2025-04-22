using System;
using UnityEngine;

public class Se_LingeringAuraOfGuidance : StatusEffect
{
	public float maxHealthHealPercentage;

	public float healInterval;

	private float _healStrength = 3f;

	private float _lastHealTime;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (base.isServer)
		{
			_lastHealTime = Time.time;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (!base.isServer || base.victim.IsNullInactiveDeadOrKnockedOut())
		{
			return;
		}
		float num = base.victim.maxHealth / 100f * maxHealthHealPercentage;
		if (base.victim is Monster monster)
		{
			switch (monster.type)
			{
			case Monster.MonsterType.Lesser:
				num *= 8f;
				break;
			case Monster.MonsterType.Normal:
				num *= 4f;
				if (monster.Status.hasUnstoppable)
				{
					num /= 3f;
				}
				break;
			case Monster.MonsterType.MiniBoss:
				num *= 1f;
				break;
			case Monster.MonsterType.Boss:
				num *= 1f;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		if (base.victim is Hero)
		{
			num *= 0.25f;
		}
		if (!(Time.time - _lastHealTime > healInterval))
		{
			return;
		}
		_lastHealTime = Time.time;
		HealData heal = new HealData(num * _healStrength);
		if (base.victim is Hero { isInCombat: false })
		{
			if (SingletonDewNetworkBehaviour<Room>.instance.didClearRoom)
			{
				_healStrength = 8f;
			}
			else
			{
				_healStrength = 1.5f;
			}
		}
		else if (base.victim.currentHealth + num * 0.25f < base.victim.maxHealth)
		{
			_healStrength = Mathf.Clamp(_healStrength - ((base.victim is Monster) ? 0.22f : 0.125f), 0.2f, 3f);
		}
		DoHeal(heal, base.victim);
	}

	private void MirrorProcessed()
	{
	}
}
