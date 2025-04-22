using UnityEngine;

public class Gem_R_Bleak : Gem
{
	public ScalingValue dmgAmp;

	public GameObject hitEffect;

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
		if (base.owner.CheckEnemyOrNeutral(target) && (target.Status.hasCold || target.Status.hasSlow || target.Status.hasRoot || target.Status.hasStun))
		{
			FxPlayNewNetworked(hitEffect, target);
			if (!data.IsAmountModifiedBy(this))
			{
				data.SetAttr(DamageAttribute.IsCrit);
				data.ApplyAmplification(GetValue(dmgAmp));
				data.SetAmountModifiedBy(this);
				NotifyUse();
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
