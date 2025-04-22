using System;
using UnityEngine;

public class Gem_R_Rigidity : Gem
{
	public ScalingValue shieldAmount;

	public float shieldDuration = 2f;

	public float coldDamageMultiplier = 2f;

	public float procCoefficientPower = 1.5f;

	public GameObject giveShieldEffect;

	private Se_GenericShield_Stacking _shield;

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.ActorEvent_OnDealDamage += new Action<EventInfoDamage>(AddShield);
			_shield = CreateStatusEffect(base.owner, new CastInfo(base.owner), delegate(Se_GenericShield_Stacking s)
			{
				s.timeout = shieldDuration;
			});
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer)
		{
			if (oldSkill != null)
			{
				oldSkill.ActorEvent_OnDealDamage -= new Action<EventInfoDamage>(AddShield);
			}
			if (!_shield.IsNullOrInactive())
			{
				_shield.Destroy();
				_shield = null;
			}
		}
	}

	private void AddShield(EventInfoDamage obj)
	{
		if (base.owner.CheckEnemyOrNeutral(obj.victim) && IsReady())
		{
			float num = GetValue(shieldAmount);
			if (obj.damage.elemental == ElementalType.Cold)
			{
				num *= coldDamageMultiplier;
			}
			_shield.AddAmount(num * Mathf.Pow(obj.damage.procCoefficient, procCoefficientPower));
			FxPlayNewNetworked(giveShieldEffect, base.owner);
			NotifyUse();
		}
	}

	private void MirrorProcessed()
	{
	}
}
