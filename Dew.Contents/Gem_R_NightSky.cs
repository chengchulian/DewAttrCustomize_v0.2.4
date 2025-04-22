using System;
using UnityEngine;

public class Gem_R_NightSky : Gem
{
	public ScalingValue perHitAtkSpd;

	public ScalingValue maxAtkSpdNormal;

	public ScalingValue maxAtkSpdDark;

	public bool useProcCoefficient;

	public float duration;

	private Se_R_NightSky_AtkSpd _atkSpd;

	private float _lastDamageTime;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			if (_atkSpd != null && _atkSpd.isActive)
			{
				_atkSpd.Destroy();
			}
			_atkSpd = CreateStatusEffect<Se_R_NightSky_AtkSpd>(base.owner, new CastInfo(base.owner));
			newOwner.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer)
		{
			if (_atkSpd != null && _atkSpd.isActive)
			{
				_atkSpd.Destroy();
			}
			_atkSpd = CreateStatusEffect<Se_R_NightSky_AtkSpd>(base.owner, new CastInfo(base.owner));
			base.numberDisplay = Mathf.RoundToInt(0f);
			ResetCooldown();
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer)
		{
			if (_atkSpd != null)
			{
				_atkSpd.Destroy();
				_atkSpd = null;
			}
			base.numberDisplay = Mathf.RoundToInt(0f);
			ResetCooldown();
			if (oldOwner != null)
			{
				oldOwner.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
			}
		}
	}

	private void EntityEventOnAttackEffectTriggered(EventInfoAttackEffect obj)
	{
		if (!(_atkSpd == null) && _atkSpd.haste != null)
		{
			_lastDamageTime = Time.time;
			if (_atkSpd.haste.strength > 0f)
			{
				StartCooldown();
			}
		}
	}

	protected override void OnDealDamage(EventInfoDamage info)
	{
		base.OnDealDamage(info);
		if (base.owner.CheckEnemyOrNeutral(info.victim) && !(_atkSpd == null))
		{
			_lastDamageTime = Time.time;
			NotifyUse();
			StartCooldown();
			float strength = _atkSpd.haste.strength;
			float num = ((info.damage.elemental == ElementalType.Dark) ? GetValue(maxAtkSpdDark) : GetValue(maxAtkSpdNormal));
			if (!(strength > num))
			{
				strength = Mathf.MoveTowards(strength, num, GetValue(perHitAtkSpd) * (useProcCoefficient ? info.damage.procCoefficient : 1f));
				_atkSpd.haste.strength = strength;
				_atkSpd.Network_effectStrength = strength / GetValue(maxAtkSpdDark);
				base.numberDisplay = Mathf.RoundToInt(_atkSpd.haste.strength);
			}
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !(base.owner == null) && !(_atkSpd == null) && _atkSpd.haste != null && Time.time - _lastDamageTime > duration && _atkSpd.haste.strength > 0f)
		{
			_atkSpd.haste.strength = 0f;
			_atkSpd.Network_effectStrength = 0f;
			base.numberDisplay = Mathf.RoundToInt(0f);
		}
	}

	private void MirrorProcessed()
	{
	}
}
