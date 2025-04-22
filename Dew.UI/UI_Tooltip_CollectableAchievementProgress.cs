using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip_CollectableAchievementProgress : UI_Tooltip_BaseObj
{
	public TextMeshProUGUI progressText;

	public Image progressFill;

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
		DewProfile.AchievementData data = DewSave.profile.achievements[achType.Name];
		if (data.isCompleted || s.alwaysUnlocked)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		progressText.text = $"{data.currentProgress:#,##0} / {data.maxProgress:#,##0}";
		progressFill.fillAmount = ((data.currentProgress == 0) ? 0f : Mathf.Clamp((float)data.currentProgress / (float)data.maxProgress, 0.05f, 0.95f));
	}
}
