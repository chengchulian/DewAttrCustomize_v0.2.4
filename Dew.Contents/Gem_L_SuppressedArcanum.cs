public class Gem_L_SuppressedArcanum : Gem
{
	public float hpThresholdRatio;

	public float shieldAmpOnLowHp;

	public float delay;

	public float waitForDashMaxTime;

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
	}

	private void ShieldProcessor(ref HealData data, Actor actor, Entity target)
	{
	}

	protected override void OnCastComplete(EventInfoCast info)
	{
	}

	private void MirrorProcessed()
	{
	}
}
