using System;

public class UI_Tooltip_CollectableAchievementDescription : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is Type t) || !(currentObjects[1] is CollectableTooltipSettings s))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		Type achType = Dew.GetRequiredAchievementOfTarget(t);
		if (achType == null)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		if (DewSave.profile.achievements[achType.Name].isCompleted || s.alwaysUnlocked)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		text.text = DewLocalization.GetAchievementDescription(achType.Name);
	}
}
