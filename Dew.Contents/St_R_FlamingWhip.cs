using System;

public class St_R_FlamingWhip : SkillTrigger
{
	public bool excludeMovementSkill;

	protected override void OnEquip(Entity newOwner)
	{
		base.OnEquip(newOwner);
		if (base.isServer)
		{
			((Hero)newOwner).ClientHeroEvent_OnSkillUse += new Action<EventInfoSkillUse>(ClientHeroEventOnSkillUse);
		}
	}

	protected override void OnUnequip(Entity formerOwner)
	{
		base.OnUnequip(formerOwner);
		if (base.isServer && !(formerOwner == null))
		{
			((Hero)formerOwner).ClientHeroEvent_OnSkillUse -= new Action<EventInfoSkillUse>(ClientHeroEventOnSkillUse);
		}
	}

	private void ClientHeroEventOnSkillUse(EventInfoSkillUse obj)
	{
		if (!(obj.skill is St_R_FlamingWhip) && (!excludeMovementSkill || obj.type != HeroSkillLocation.Movement))
		{
			ResetCooldown();
		}
	}

	private void MirrorProcessed()
	{
	}
}
