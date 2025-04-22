public class Gem_C_Lethality : Gem
{
	public ScalingValue dmgAmp;

	public float healthThreshold;

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.dealtDamageProcessor.Add(Amplify);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && oldSkill != null)
		{
			oldSkill.dealtDamageProcessor.Remove(Amplify);
		}
	}

	private void Amplify(ref DamageData data, Actor actor, Entity target)
	{
		if (base.owner.CheckEnemyOrNeutral(target) && !data.IsAmountModifiedBy(this) && !(target.Status.currentHealth / target.Status.maxHealth < healthThreshold))
		{
			data.SetAttr(DamageAttribute.IsCrit);
			data.ApplyAmplification(GetValue(dmgAmp));
			data.SetAmountModifiedBy(this);
			NotifyUse();
		}
	}

	private void MirrorProcessed()
	{
	}
}
