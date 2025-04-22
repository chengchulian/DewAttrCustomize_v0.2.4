using System.Collections.Generic;
using UnityEngine;

public class UI_Tooltip_SkillStarDescriber : UI_Tooltip_BaseObj
{
	public UI_Tooltip_SkillStarDescriber_Item itemPrefab;

	public int objIndexSkill;

	private void DoResultScreen(DewGameResult data, int playerIndex, HeroSkillLocation loc)
	{
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
		bool hasRelated = false;
		DewGameResult.PlayerData player = data.players[playerIndex];
		foreach (DewGameResult.SkillData skill in player.skills)
		{
			if (skill.loc != loc)
			{
				continue;
			}
			for (int i = 0; i < player.stars.Count; i++)
			{
				if (Dew.oldStarsByName[player.stars[i].type].IsSkillRelated(skill.name))
				{
					Object.Instantiate(itemPrefab, base.transform).Setup(player.stars[i].type, player.capturedStarTooltipFields);
					hasRelated = true;
				}
			}
		}
		base.gameObject.SetActive(hasRelated);
	}

	private void DoLobby(SkillTrigger skill)
	{
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
		bool hasRelated = false;
		foreach (KeyValuePair<string, DewProfile.StarData> p in DewSave.profile.stars)
		{
			if (p.Value.level > 0 && skill != null && Dew.oldStarsByName[p.Key].IsSkillRelated(skill))
			{
				Object.Instantiate(itemPrefab, base.transform).Setup(p.Key, p.Value.level, null);
				hasRelated = true;
			}
		}
		base.gameObject.SetActive(hasRelated);
	}

	private void DoInGame(SkillTrigger skill)
	{
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
		bool hasRelated = false;
		for (int i = 0; i < skill.owner.owner.stars.Count; i++)
		{
			PlayerStarItem starData = skill.owner.owner.stars[i];
			if (Dew.oldStarsByName[starData.type].IsSkillRelated(skill))
			{
				Object.Instantiate(itemPrefab, base.transform).Setup(starData.type, starData.level, skill.owner);
				hasRelated = true;
			}
		}
		base.gameObject.SetActive(hasRelated);
	}

	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult data && currentObjects[1] is int playerIndex && currentObjects[2] is HeroSkillLocation resultSkillType)
		{
			DoResultScreen(data, playerIndex, resultSkillType);
			return;
		}
		Hero hero = ((DewPlayer.local != null) ? DewPlayer.local.hero : null);
		if (currentObjects.Count == 2 && currentObjects[1] is Hero h)
		{
			hero = h;
		}
		if (currentObjects.Count > objIndexSkill && currentObjects[objIndexSkill] is SkillTrigger skill)
		{
			if (hero != null && hero.Skill.TryGetSkillLocation(skill, out var _))
			{
				DoInGame(skill);
			}
			else
			{
				DoLobby(skill);
			}
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
