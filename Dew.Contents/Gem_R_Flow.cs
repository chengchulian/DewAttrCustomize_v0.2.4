using System;
using UnityEngine;

public class Gem_R_Flow : Gem
{
	public GameObject reduceCooldownEffect;

	public ScalingValue reduceRatio;

	public float ultRatio;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			newOwner.ActorEvent_OnDealDamage += new Action<EventInfoDamage>(OnOwnerDealDamage);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer && oldOwner != null)
		{
			oldOwner.ActorEvent_OnDealDamage -= new Action<EventInfoDamage>(OnOwnerDealDamage);
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.ClientTriggerEvent_OnCooldownReduced += new Action<int, float>(OnSkillCooldownReduced);
			newSkill.ClientTriggerEvent_OnCooldownReducedByRatio += new Action<int, float>(OnSkillCooldownReducedByRatio);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && oldSkill != null)
		{
			oldSkill.ClientTriggerEvent_OnCooldownReduced -= new Action<int, float>(OnSkillCooldownReduced);
			oldSkill.ClientTriggerEvent_OnCooldownReducedByRatio -= new Action<int, float>(OnSkillCooldownReducedByRatio);
		}
	}

	private void OnSkillCooldownReduced(int configIndex, float amount)
	{
		if (configIndex != 0 || !base.isValid || !base.owner.Skill.TryGetSkillLocation(base.skill, out var type))
		{
			return;
		}
		foreach (Gem item in base.owner.Skill.GetGemsInSkill(type))
		{
			item.ApplyCooldownReduction(amount);
		}
		NotifyUse();
	}

	private void OnSkillCooldownReducedByRatio(int configIndex, float ratio)
	{
		if (configIndex != 0 || !base.isValid || !base.owner.Skill.TryGetSkillLocation(base.skill, out var type))
		{
			return;
		}
		foreach (Gem item in base.owner.Skill.GetGemsInSkill(type))
		{
			item.ApplyCooldownReductionByRatio(ratio);
		}
		NotifyUse();
	}

	private void OnOwnerDealDamage(EventInfoDamage obj)
	{
		if (!(base.skill == null) && base.owner.CheckEnemyOrNeutral(obj.victim) && obj.damage.elemental == ElementalType.Light)
		{
			NotifyUse();
			FxPlayNewNetworked(reduceCooldownEffect, base.owner);
			float num = GetValue(reduceRatio) * obj.damage.procCoefficient;
			if (base.skill.type == SkillType.Ultimate)
			{
				num *= ultRatio;
			}
			base.skill.ApplyCooldownReductionByRatio(num);
		}
	}

	private void MirrorProcessed()
	{
	}
}
