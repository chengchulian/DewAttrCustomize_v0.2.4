using TMPro;
using UnityEngine;

public class UI_Tooltip_ObjRarityText : UI_Tooltip_BaseObj
{
	public bool doSuffix = true;

	public override void OnSetup()
	{
		base.OnSetup();
		bool isDodgeAbil = false;
		Rarity rarity;
		string template;
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult data && currentObjects[1] is int playerIndex0 && currentObjects[2] is GemLocation gemLocation)
		{
			if (!data.players[playerIndex0].TryGetGemData(gemLocation, out var gemData))
			{
				return;
			}
			rarity = gemData.GetGem().rarity;
			template = "InGame_Tooltip_Essence";
		}
		else if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult result && currentObjects[1] is int playerIndex1 && currentObjects[2] is HeroSkillLocation skillType)
		{
			if (!result.players[playerIndex1].TryGetSkillData(skillType, out var skillData))
			{
				return;
			}
			rarity = skillData.GetSkillTrigger().rarity;
			template = "InGame_Tooltip_Skill";
			isDodgeAbil = skillType == HeroSkillLocation.Movement;
		}
		else if (base.currentObject is SkillTrigger skill)
		{
			rarity = skill.rarity;
			template = "InGame_Tooltip_Skill";
			isDodgeAbil = skill.GetType().Name.Contains("_M_");
		}
		else
		{
			if (!(base.currentObject is Gem gem))
			{
				return;
			}
			rarity = gem.rarity;
			template = "InGame_Tooltip_Essence";
		}
		Color color = Dew.GetRarityColor(rarity);
		text.color = color;
		text.text = DewLocalization.GetUIValue(isDodgeAbil ? "InGame_Tooltip_DodgeAbility" : $"InGame_Tooltip_Rarity_{rarity}");
		if (!isDodgeAbil && doSuffix)
		{
			TextMeshProUGUI textMeshProUGUI = text;
			textMeshProUGUI.text = textMeshProUGUI.text + " " + DewLocalization.GetUIValue(template);
		}
	}
}
