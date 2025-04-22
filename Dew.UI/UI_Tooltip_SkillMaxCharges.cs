public class UI_Tooltip_SkillMaxCharges : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult result && currentObjects[1] is int playerIndex && currentObjects[2] is HeroSkillLocation skillType)
		{
			if (!result.players[playerIndex].TryGetSkillData(skillType, out var skillData) || skillData.maxCharges < 2)
			{
				text.enabled = false;
				return;
			}
			text.enabled = true;
			text.text = string.Format(DewLocalization.GetUIValue("SkillTooltip_MaxCharges"), skillData.maxCharges);
		}
		else if (!(base.currentObject is SkillTrigger skill) || skill.currentConfig.maxCharges < 2)
		{
			text.enabled = false;
		}
		else
		{
			text.enabled = true;
			text.text = string.Format(DewLocalization.GetUIValue("SkillTooltip_MaxCharges"), skill.currentConfig.maxCharges);
		}
	}
}
