public class UI_Tooltip_QuestDescription : UI_Tooltip_BaseObj
{
	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is DewQuest quest))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		text.text = ((!string.IsNullOrEmpty(quest.questDetailedDescriptionRaw)) ? quest.questDetailedDescriptionRaw : quest.questShortDescriptionRaw);
	}
}
