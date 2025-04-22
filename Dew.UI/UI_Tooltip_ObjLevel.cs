public class UI_Tooltip_ObjLevel : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult data && currentObjects[1] is int playerIndex0 && currentObjects[2] is GemLocation gemLocation)
		{
			if (data.players[playerIndex0].TryGetGemData(gemLocation, out var gemData))
			{
				text.text = gemData.quality.ToString("0");
			}
		}
		else if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult result && currentObjects[1] is int playerIndex1 && currentObjects[2] is HeroSkillLocation skillType)
		{
			if (result.players[playerIndex1].TryGetSkillData(skillType, out var skillData))
			{
				text.text = skillData.level.ToString("0");
			}
		}
		else if (base.currentObject is SkillTrigger skill)
		{
			text.text = skill.level.ToString("0");
		}
		else if (base.currentObject is Gem gem)
		{
			text.text = gem.effectiveLevel.ToString("0");
		}
		else
		{
			text.text = "1";
		}
	}
}
