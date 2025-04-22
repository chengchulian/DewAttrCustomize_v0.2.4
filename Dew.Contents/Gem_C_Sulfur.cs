using UnityEngine;

public class Gem_C_Sulfur : Gem
{
	public ScalingValue ampAmount;

	public GameObject hitEffect;

	public ScalingValue fireAmpBonus;

	private StatBonus _stats;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			_stats = newOwner.Status.AddStatBonus(new StatBonus
			{
				fireEffectAmpFlat = GetValue(fireAmpBonus)
			});
		}
	}

	protected override void OnQualityChange(int oldQuality, int newQuality)
	{
		base.OnQualityChange(oldQuality, newQuality);
		if (base.isServer && _stats != null)
		{
			_stats.fireEffectAmpFlat = GetValue(fireAmpBonus);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer && oldOwner != null && _stats != null)
		{
			oldOwner.Status.RemoveStatBonus(_stats);
			_stats = null;
		}
	}

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.dealtDamageProcessor.Add(Setablazee, 50);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && !(oldSkill == null))
		{
			oldSkill.dealtDamageProcessor.Remove(Setablazee);
		}
	}

	private void Setablazee(ref DamageData data, Actor actor, Entity target)
	{
		if (base.owner.CheckEnemyOrNeutral(target))
		{
			data.SetElemental(ElementalType.Fire);
			if (!data.IsAmountModifiedBy(this))
			{
				data.ApplyAmplification(GetValue(ampAmount));
				data.SetAmountModifiedBy(this);
				FxPlayNewNetworked(hitEffect, target);
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
