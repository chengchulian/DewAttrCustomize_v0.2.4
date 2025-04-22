public class UI_Tooltip_SkillCooldown : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult result && currentObjects[1] is int playerIndex1 && currentObjects[2] is HeroSkillLocation skillType)
		{
			if (!result.players[playerIndex1].TryGetSkillData(skillType, out var skillData))
			{
				text.enabled = false;
			}
			else if (skillData.type == SkillType.Ultimate)
			{
				text.enabled = true;
				text.text = DewLocalization.GetUIValue("InGame_Tooltip_Ultimate");
			}
			else if (!skillData.GetSkillTrigger().configs[0].isActive)
			{
				text.enabled = true;
				text.text = DewLocalization.GetUIValue("InGame_Tooltip_Skill_Passive");
			}
			else if (skillData.cooldownTime < 0.01f)
			{
				text.enabled = false;
			}
			else
			{
				text.enabled = true;
				text.text = string.Format(DewLocalization.GetUIValue("SkillTooltip_Cooldown"), skillData.cooldownTime.ToString("0.#"));
			}
			return;
		}
		if (!(base.currentObject is SkillTrigger skill))
		{
			text.enabled = false;
			return;
		}
		text.enabled = true;
		if (skill.type == SkillType.Ultimate && skill.currentConfigIndex == 0)
		{
			text.text = DewLocalization.GetUIValue("InGame_Tooltip_Ultimate");
		}
		else if (!skill.currentConfig.isActive)
		{
			text.text = DewLocalization.GetUIValue("InGame_Tooltip_Skill_Passive");
		}
		else if (currentObjects.Count == 3 && currentObjects[1] is int fromLevel && currentObjects[2] is int toLevel)
		{
			float fromCool = skill.GetCooldownTimeOnLevel(fromLevel);
			float toCool = skill.GetCooldownTimeOnLevel(toLevel);
			if (fromCool < 0.01f && toCool < 0.01f)
			{
				text.enabled = false;
				return;
			}
			string fromCooldownText = string.Format(DewLocalization.GetUIValue("SkillTooltip_Cooldown"), fromCool.ToString("0.#"));
			string toCooldownText = string.Format(DewLocalization.GetUIValue("SkillTooltip_Cooldown"), toCool.ToString("0.#"));
			if (fromCooldownText != toCooldownText)
			{
				text.text = DewLocalization.RenderChangedValue(fromCooldownText, toCooldownText);
			}
			else
			{
				text.text = fromCooldownText;
			}
		}
		else if (currentObjects.Count == 2 && currentObjects[1] is int level)
		{
			float cool = skill.GetCooldownTimeOnLevel(level);
			if (cool < 0.01f)
			{
				text.enabled = false;
			}
			else
			{
				text.text = string.Format(DewLocalization.GetUIValue("SkillTooltip_Cooldown"), cool.ToString("0.#"));
			}
		}
		else if (currentObjects.Count == 3 && currentObjects[1] is int lvl && currentObjects[2] is string info && info == "RAW")
		{
			float cool2 = skill.GetCooldownTimeOnLevel(lvl);
			if (cool2 < 0.01f)
			{
				text.enabled = false;
			}
			else
			{
				text.text = string.Format(DewLocalization.GetUIValue("SkillTooltip_Cooldown"), cool2.ToString("0.#"));
			}
		}
		else
		{
			float time = ((!(skill.netIdentity == null)) ? skill.currentConfigMaxCooldownTime : skill.configs[0].cooldownTime);
			if (time < 0.01f)
			{
				text.enabled = false;
			}
			else
			{
				text.text = string.Format(DewLocalization.GetUIValue("SkillTooltip_Cooldown"), time.ToString("0.#"));
			}
		}
	}
}
