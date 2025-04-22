using UnityEngine.UI;

public class UI_Tooltip_ObjIcon : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		Image image = GetComponent<Image>();
		if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult data && currentObjects[1] is int playerIndex0 && currentObjects[2] is GemLocation gemLocation)
		{
			if (data.players[playerIndex0].TryGetGemData(gemLocation, out var gemData))
			{
				Gem gem = DewResources.GetByShortTypeName<Gem>(gemData.name);
				image.sprite = gem.icon;
			}
		}
		else if (currentObjects.Count == 3 && currentObjects[0] is DewGameResult result && currentObjects[1] is int playerIndex1 && currentObjects[2] is HeroSkillLocation skillType)
		{
			if (result.players[playerIndex1].TryGetSkillData(skillType, out var skillData))
			{
				SkillTrigger skill = DewResources.GetByShortTypeName<SkillTrigger>(skillData.name);
				image.sprite = skill.configs[0].triggerIcon;
			}
		}
		else if (base.currentObject is SkillTrigger s)
		{
			image.sprite = s.configs[0].triggerIcon;
		}
		else if (base.currentObject is Gem g)
		{
			image.sprite = g.icon;
		}
		else if (base.currentObject is Artifact a)
		{
			image.sprite = a.icon;
		}
	}
}
