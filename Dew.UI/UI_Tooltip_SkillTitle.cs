public class UI_Tooltip_SkillTitle : UI_Tooltip_BaseObj
{
	public bool enableGemTemplate;

	public override void OnSetup()
	{
		base.OnSetup();
		if (!DoResultScreenTooltip())
		{
			DoInGameTooltip();
		}
	}

	private bool DoResultScreenTooltip()
	{
		if (currentObjects.Count != 3 || !(currentObjects[0] is DewGameResult result) || !(currentObjects[1] is int playerIndex) || !(currentObjects[2] is HeroSkillLocation skillType))
		{
			return false;
		}
		DewGameResult.PlayerData playerData = result.players[playerIndex];
		if (!playerData.TryGetSkillData(skillType, out var skillData))
		{
			return true;
		}
		SkillTrigger skill = skillData.GetSkillTrigger();
		string color = Dew.GetRarityColorHex(skill.rarity);
		string skillName = DewLocalization.GetSkillName(DewLocalization.GetSkillKey(skill.GetType()), skill.currentConfigIndex);
		DewGameResult.GemData? firstGem = null;
		foreach (DewGameResult.GemData g in playerData.gems)
		{
			if (g.location.skill == skillType && (!firstGem.HasValue || firstGem.Value.location.index > g.location.index))
			{
				firstGem = g;
			}
		}
		if (firstGem.HasValue)
		{
			skillName = string.Format(DewLocalization.GetGemTemplate(DewLocalization.GetGemKey(firstGem.Value.name)), skillName);
		}
		text.text = "<color=" + color + ">" + string.Format(DewLocalization.GetSkillLevelTemplate(skillData.level, null), skillName) + "</color>";
		return true;
	}

	private void DoInGameTooltip()
	{
		if (!(base.currentObject is SkillTrigger skill))
		{
			return;
		}
		Hero hero = ((DewPlayer.local != null) ? DewPlayer.local.hero : null);
		if (currentObjects.Count == 2 && currentObjects[1] is Hero h)
		{
			hero = h;
		}
		else if (currentObjects.Count == 3 && currentObjects[1] is int && currentObjects[2] is string info && info == "RAW")
		{
			hero = null;
		}
		string color = Dew.GetRarityColorHex(skill.rarity);
		string skillName = DewLocalization.GetSkillName(DewLocalization.GetSkillKey(skill.GetType()), skill.currentConfigIndex);
		if (enableGemTemplate && hero != null && hero.Skill.TryGetSkillLocation(skill, out var loc))
		{
			Gem firstGem = hero.Skill.GetFirstGem(loc);
			if (firstGem != null && firstGem != ManagerBase<EditSkillManager>.instance.draggingObject)
			{
				skillName = string.Format(DewLocalization.GetGemTemplate(DewLocalization.GetGemKey(firstGem.GetType())), skillName);
			}
		}
		int level = skill.level;
		int? toLevel = null;
		if (currentObjects.Count == 2 && currentObjects[1] is int lvl)
		{
			level = lvl;
		}
		if (currentObjects.Count == 3 && currentObjects[1] is int fromLevel && currentObjects[2] is int toLvl)
		{
			level = fromLevel;
			toLevel = toLvl;
		}
		else if (currentObjects.Count == 3 && currentObjects[1] is int lll && currentObjects[2] is string info2 && info2 == "RAW")
		{
			level = lll;
		}
		text.text = "<color=" + color + ">" + string.Format(DewLocalization.GetSkillLevelTemplate(level, toLevel), skillName) + "</color>";
	}
}
