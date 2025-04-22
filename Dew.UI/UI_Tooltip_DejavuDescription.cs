using UnityEngine;

public class UI_Tooltip_DejavuDescription : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (currentObjects.Count != 2 || !(currentObjects[1] is string str) || str != "Dejavu")
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		int currentCount = DewSave.profile.itemStatistics[currentObjects[0].GetType().Name].wins;
		if (currentCount <= 0)
		{
			text.text = DewLocalization.GetUIValue((currentObjects[0] is SkillTrigger) ? "Dejavu_NeedToReachPureWhiteDream_Memory" : "Dejavu_NeedToReachPureWhiteDream_Essence");
			return;
		}
		int maxCount = Dew.GetDejavuMaxWins((Object)currentObjects[0]);
		string stats = string.Format(DewLocalization.GetUIValue("Dejavu_ReachPureWhiteDreamStats"), currentCount, maxCount);
		text.text = stats;
	}
}
