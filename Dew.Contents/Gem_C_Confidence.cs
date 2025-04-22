using UnityEngine;

public class Gem_C_Confidence : Gem
{
	public GameObject fxRange;

	public GameObject fxHit;

	public ScalingValue dmgAmp;

	public float radius;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (newOwner.isOwned)
		{
			FxPlay(fxRange, newOwner);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		FxStop(fxRange);
	}

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
		if (base.owner.CheckEnemyOrNeutral(target) && !(Vector2.Distance(base.owner.position.ToXY(), target.position.ToXY()) > radius + target.Control.outerRadius))
		{
			FxPlayNewNetworked(fxHit, target);
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
