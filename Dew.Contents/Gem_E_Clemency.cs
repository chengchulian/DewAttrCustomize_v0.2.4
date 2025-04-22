using System;
using UnityEngine;

public class Gem_E_Clemency : Gem
{
	public GameObject healEffect;

	public ScalingValue conversionRatio;

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.ActorEvent_OnDealDamage += new Action<EventInfoDamage>(ConvertToHeal);
		}
	}

	private void ConvertToHeal(EventInfoDamage obj)
	{
		if (!(base.owner == null) && !obj.chain.DidReact(this) && base.owner.CheckEnemyOrNeutral(obj.victim))
		{
			Heal(obj.damage.amount * GetValue(conversionRatio)).SetAmountOrigin(obj.damage).Dispatch(base.owner, obj.chain.New(this));
			FxPlayNewNetworked(healEffect, base.owner);
			NotifyUse();
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && oldSkill != null)
		{
			oldSkill.ActorEvent_OnDealDamage -= new Action<EventInfoDamage>(ConvertToHeal);
		}
	}

	private void MirrorProcessed()
	{
	}
}
