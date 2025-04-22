using System;
using UnityEngine;

public class UI_Tooltip_CollectableTargetName : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is Type t) || !(currentObjects[1] is CollectableTooltipSettings settings))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		Type achType = Dew.GetRequiredAchievementOfTarget(t);
		if (achType != null && DewSave.profile.achievements.TryGetValue(achType.Name, out var data) && !data.isCompleted && !settings.alwaysUnlocked)
		{
			text.text = "???";
		}
		else if (t.IsSubclassOf(typeof(SkillTrigger)))
		{
			SkillTrigger s = DewResources.GetByShortTypeName<SkillTrigger>(t.Name);
			text.text = "<color=" + Dew.GetRarityColorHex(s.rarity) + ">" + DewLocalization.GetSkillName(DewLocalization.GetSkillKey(t.Name), 0) + "</color>";
		}
		else if (t.IsSubclassOf(typeof(Gem)))
		{
			Gem s2 = DewResources.GetByShortTypeName<Gem>(t.Name);
			text.text = "<color=" + Dew.GetRarityColorHex(s2.rarity) + ">" + DewLocalization.GetGemName(DewLocalization.GetGemKey(t.Name)) + "</color>";
		}
		else if (t.IsSubclassOf(typeof(Hero)))
		{
			Color.RGBToHSV(DewResources.GetByShortTypeName<Hero>(t.Name).mainColor, out var h, out var s3, out var _);
			Color color = Color.HSVToRGB(h, s3 * 0.8f, 1f, hdr: false);
			text.text = "<color=" + Dew.GetHex(color) + ">" + DewLocalization.GetUIValue(t.Name + "_Name") + ", " + DewLocalization.GetUIValue(t.Name + "_Subtitle") + "</color>";
		}
		else if (t.IsSubclassOf(typeof(LucidDream)))
		{
			LucidDream l = DewResources.GetByShortTypeName<LucidDream>(t.Name);
			text.text = "<color=white>" + DewLocalization.GetUIValue("LucidDream") + "</color> <color=" + Dew.GetHex(l.color) + ">" + DewLocalization.GetUIValue(t.Name + "_Name") + "</color>\n" + DewLocalization.GetUIValue(t.Name + "_Description");
		}
		else
		{
			text.text = "!Unknown Target";
		}
	}
}
