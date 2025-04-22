public class UI_Tooltip_QuestProgress : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (base.currentObject is DewQuest { progressType: not QuestProgressType.Hidden } quest)
		{
			base.gameObject.SetActive(value: true);
			text.text = string.Format(DewLocalization.GetUIValue("InGame_Quest_ProgressCondition_" + quest.progressType), quest.currentProgress);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
