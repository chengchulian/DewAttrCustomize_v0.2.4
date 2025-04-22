using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip_ObjRarityColor : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		Image image = GetComponent<Image>();
		TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
		Color color;
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult data && currentObjects[1] is int playerIndex0 && currentObjects[2] is GemLocation gemLocation)
		{
			if (!data.players[playerIndex0].TryGetGemData(gemLocation, out var gemData))
			{
				return;
			}
			color = Dew.GetRarityColor(gemData.GetGem().rarity);
		}
		else if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult result && currentObjects[1] is int playerIndex1 && currentObjects[2] is HeroSkillLocation skillType)
		{
			if (!result.players[playerIndex1].TryGetSkillData(skillType, out var skillData))
			{
				return;
			}
			color = Dew.GetRarityColor(skillData.GetSkillTrigger().rarity);
		}
		else if (base.currentObject is SkillTrigger s)
		{
			color = Dew.GetRarityColor(s.rarity);
		}
		else if (base.currentObject is Gem g)
		{
			color = Dew.GetRarityColor(g.rarity);
		}
		else
		{
			if (!(base.currentObject is Artifact a))
			{
				return;
			}
			color = a.mainColor;
		}
		if (image != null)
		{
			Color c = color;
			c.a = image.color.a;
			image.color = c;
		}
		else if (text != null)
		{
			Color c2 = color;
			c2.a = text.color.a;
			text.color = c2;
		}
	}
}
