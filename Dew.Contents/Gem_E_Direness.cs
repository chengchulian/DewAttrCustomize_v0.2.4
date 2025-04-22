using UnityEngine;

public class Gem_E_Direness : Gem
{
	public float maxAbilityHasteHpThreshold;

	public ScalingValue maxAbilityHaste;

	public float updateInterval;

	private float _lastUpdateTime;

	private SkillBonus _bonus;

	public float maxReducedRatio => 1f - 1f / (1f + GetValue(maxAbilityHaste) * 0.01f);

	public override void OnEquipSkill(SkillTrigger newSkill)
	{
		base.OnEquipSkill(newSkill);
		if (base.isServer)
		{
			_bonus = new SkillBonus();
			newSkill.AddSkillBonus(_bonus);
		}
	}

	public override void OnUnequipSkill(SkillTrigger oldSkill)
	{
		base.OnUnequipSkill(oldSkill);
		if (base.isServer && !(oldSkill == null))
		{
			oldSkill.RemoveSkillBonus(_bonus);
			_bonus = null;
		}
	}

	protected override void ActiveLogicUpdate(float dt)
	{
		base.ActiveLogicUpdate(dt);
		if (base.isServer && !(base.owner == null) && _bonus != null && !(Time.time - _lastUpdateTime < updateInterval))
		{
			_lastUpdateTime = Time.time;
			float num = base.owner.currentHealth / base.owner.maxHealth;
			float num2 = 1f - Mathf.Clamp01((num - maxAbilityHasteHpThreshold) / (1f - maxAbilityHasteHpThreshold));
			float num3 = maxReducedRatio * num2;
			_bonus.cooldownMultiplier = 1f - num3;
		}
	}

	private void MirrorProcessed()
	{
	}
}
