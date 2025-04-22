using UnityEngine;

public class UI_Tooltip_QuestType : UI_Tooltip_BaseObj
{
	public GameObject goalObject;

	public GameObject curseObject;

	public GameObject tutorialObject;

	public GameObject questObject;

	public override void OnSetup()
	{
		base.OnSetup();
		if (!(base.currentObject is DewQuest quest))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		goalObject.SetActive(quest.type == QuestType.Goal);
		curseObject.SetActive(quest.type == QuestType.Curse);
		tutorialObject.SetActive(quest.type == QuestType.Tutorial);
		questObject.SetActive(quest.type == QuestType.Quest);
	}
}
