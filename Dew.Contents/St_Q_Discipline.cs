using System;

public class St_Q_Discipline : SkillTrigger
{
	public float cooldownReductionRatio;

	protected override void OnEquip(Entity newOwner)
	{
		base.OnEquip(newOwner);
		if (base.isServer)
		{
			newOwner.EntityEvent_OnAttackEffectTriggered += new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
		}
	}

	protected override void OnUnequip(Entity formerOwner)
	{
		base.OnUnequip(formerOwner);
		if (base.isServer && formerOwner != null)
		{
			formerOwner.EntityEvent_OnAttackEffectTriggered -= new Action<EventInfoAttackEffect>(EntityEventOnAttackEffectTriggered);
		}
	}

	private void EntityEventOnAttackEffectTriggered(EventInfoAttackEffect obj)
	{
		ApplyCooldownReductionByRatio(obj.strength * cooldownReductionRatio);
	}

	private void MirrorProcessed()
	{
	}
}
