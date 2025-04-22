public class Gem_C_Guidance : Gem
{
	public ScalingValue healAmp;

	public float shieldRatio;

	public float shieldDuration = 3f;

	private Se_GenericShield_Stacking _shield;

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			newSkill.dealtHealProcessor.Add(HealAmp);
			_shield = CreateStatusEffect(base.owner, new CastInfo(base.owner), delegate(Se_GenericShield_Stacking s)
			{
				s.timeout = shieldDuration;
			});
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer)
		{
			if (oldSkill != null)
			{
				oldSkill.dealtHealProcessor.Remove(HealAmp);
			}
			if (!_shield.IsNullOrInactive())
			{
				_shield.Destroy();
				_shield = null;
			}
		}
	}

	private void HealAmp(ref HealData data, Actor actor, Entity target)
	{
		data.ApplyAmplification(GetValue(healAmp));
	}

	protected override void OnDoHeal(EventInfoHeal obj)
	{
		base.OnDoHeal(obj);
		if (!(obj.discardedAmount <= 0f))
		{
			_shield.AddAmount(obj.discardedAmount * shieldRatio);
		}
	}

	private void MirrorProcessed()
	{
	}
}
