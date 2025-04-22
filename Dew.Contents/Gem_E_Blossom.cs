using UnityEngine;

public class Gem_E_Blossom : Gem
{
	public GameObject healEffect;

	public ScalingValue conversionRatio;

	public float damageReduction;

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
	}

	private void Processor(ref DamageData data, Actor actor, Entity target)
	{
	}

	private void ConvertToHeal(EventInfoDamage obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
