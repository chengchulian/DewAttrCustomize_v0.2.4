using System.Collections.Generic;

public class Se_Star_D_UltimateChargeSpeed : StarEffect
{
	public float[] chargeSpeedBonusRatio;

	private Dictionary<SkillTrigger, SkillBonus> _appliedBonuses;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void ClientHeroEventOnSkillEquip(SkillTrigger obj)
	{
	}

	private void ClientHeroEventOnSkillUnequip(SkillTrigger obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
