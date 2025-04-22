using System;

public class Gem_R_Momentum : Gem
{
	public ScalingValue reductionAmount;

	public override void OnEquipGem(Hero newOwner)
	{
		base.OnEquipGem(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(ReduceCooldown);
		}
	}

	public override void OnUnequipGem(Hero oldOwner)
	{
		base.OnUnequipGem(oldOwner);
		if (base.isServer && oldOwner != null)
		{
			oldOwner.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(ReduceCooldown);
		}
	}

	private void ReduceCooldown(EventInfoAttackEffect obj)
	{
		if (!(base.skill == null))
		{
			base.skill.ApplyCooldownReduction(GetValue(reductionAmount) * obj.strength);
			NotifyUse();
		}
	}

	private void MirrorProcessed()
	{
	}
}
