public class St_R_DangerousTheory : SkillTrigger
{
	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !(base.owner == null))
		{
			base.fillAmount = ((CanBeReserved() && base.owner.currentHealth / base.owner.maxHealth < 0.59f) ? 1f : 0f);
		}
	}

	private void MirrorProcessed()
	{
	}
}
