using System.Collections.Generic;
using DewInternal;

public class UI_Tooltip_SkillDescription : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult result && currentObjects[1] is int playerIndex && currentObjects[2] is HeroSkillLocation skillType)
		{
			if (result.players[playerIndex].TryGetSkillData(skillType, out var skillData))
			{
				List<LocaleNode> nodes0 = DewLocalization.GetSkillDescription(DewLocalization.GetSkillKey(skillData.name), 0);
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes0, new DewLocalization.DescriptionSettings
				{
					contextObject = skillData.GetSkillTrigger(),
					currentLevel = skillData.level,
					stats = new DewLocalization.EntityStats(result.players[playerIndex]),
					capturedFields = skillData.capturedTooltipFields
				});
			}
		}
		else if (base.currentObject is SkillTrigger skill)
		{
			List<LocaleNode> nodes1 = DewLocalization.GetSkillDescription(DewLocalization.GetSkillKey(skill.GetType()), skill.currentConfigIndex);
			if (currentObjects.Count == 3 && currentObjects[1] is int fromLevel && currentObjects[2] is int toLevel)
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes1, new DewLocalization.DescriptionSettings
				{
					contextEntity = ((DewPlayer.local == null) ? null : DewPlayer.local.hero),
					contextObject = skill,
					previousLevel = fromLevel,
					currentLevel = toLevel,
					showLevelScaling = true
				});
			}
			else if (currentObjects.Count == 2 && currentObjects[1] is int level)
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes1, new DewLocalization.DescriptionSettings
				{
					contextEntity = ((DewPlayer.local == null) ? null : DewPlayer.local.hero),
					contextObject = skill,
					currentLevel = level
				});
			}
			else if (currentObjects.Count == 3 && currentObjects[1] is int lvl && currentObjects[2] is string info && info == "RAW")
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes1, new DewLocalization.DescriptionSettings
				{
					contextEntity = null,
					contextObject = skill,
					currentLevel = lvl
				});
			}
			else if (currentObjects.Count == 2 && currentObjects[1] is Hero h)
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes1, new DewLocalization.DescriptionSettings
				{
					contextEntity = h,
					contextObject = skill
				});
			}
			else
			{
				text.text = DewLocalization.ConvertDescriptionNodesToText(nodes1, new DewLocalization.DescriptionSettings
				{
					contextEntity = ((DewPlayer.local == null) ? null : DewPlayer.local.hero),
					contextObject = skill
				});
			}
		}
	}
}
