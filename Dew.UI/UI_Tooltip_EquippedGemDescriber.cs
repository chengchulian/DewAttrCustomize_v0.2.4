using System.Collections.Generic;
using UnityEngine;

public class UI_Tooltip_EquippedGemDescriber : UI_Tooltip_BaseObj
{
	public UI_Tooltip_EquippedGemDescriber_Item removedDescriber;

	public UI_Tooltip_EquippedGemDescriber_Item defaultDescriber;

	public int objIndexSkill;

	public int objIndexPrevGem;

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
		if (currentObjects.Count != 3 || !(currentObjects[0] is DewGameResult data) || !(currentObjects[1] is int playerIndex) || !(currentObjects[2] is HeroSkillLocation resultSkillType))
		{
			return false;
		}
		if (!data.players[playerIndex].TryGetSkillData(resultSkillType, out var _))
		{
			base.gameObject.SetActive(value: false);
			return true;
		}
		base.gameObject.SetActive(value: true);
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
		List<DewGameResult.GemData> gems = new List<DewGameResult.GemData>();
		foreach (DewGameResult.GemData g in data.players[playerIndex].gems)
		{
			if (g.location.skill == resultSkillType)
			{
				gems.Add(g);
			}
		}
		gems.Sort((DewGameResult.GemData x, DewGameResult.GemData y) => x.location.index.CompareTo(y.location.index));
		bool gemExists = false;
		foreach (DewGameResult.GemData g2 in gems)
		{
			Object.Instantiate(defaultDescriber, base.transform).Setup(g2, data.players[playerIndex]);
			gemExists = true;
		}
		if (!gemExists)
		{
			base.gameObject.SetActive(value: false);
		}
		return true;
	}

	private void DoInGameTooltip()
	{
		Hero hero = ((DewPlayer.local != null) ? DewPlayer.local.hero : null);
		if (currentObjects.Count == 2 && currentObjects[1] is Hero h)
		{
			hero = h;
		}
		if (hero == null)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		if (!(currentObjects[objIndexSkill] is SkillTrigger skill) || !hero.Skill.TryGetSkillLocation(skill, out var skillType) || skill == ManagerBase<EditSkillManager>.instance.draggingObject || hero.Skill.GetMaxGemCount(skillType) <= 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		foreach (Transform item in base.transform)
		{
			Object.Destroy(item.gameObject);
		}
		Gem prevGem = currentObjects[objIndexPrevGem] as Gem;
		bool gemExists = false;
		foreach (Gem g in hero.Skill.GetGemsInSkill(skillType))
		{
			if (!(g == null) && !(g == ManagerBase<EditSkillManager>.instance.draggingObject))
			{
				Object.Instantiate((prevGem == g) ? removedDescriber : defaultDescriber, base.transform).Setup(g, hero);
				gemExists = true;
			}
		}
		if (!gemExists)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
