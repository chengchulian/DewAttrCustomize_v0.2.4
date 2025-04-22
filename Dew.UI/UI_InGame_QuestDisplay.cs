using System;
using UnityEngine;

public class UI_InGame_QuestDisplay : MonoBehaviour
{
	public UI_InGame_QuestDisplay_Item itemPrefab;

	public Transform itemParent;

	private void Start()
	{
		NetworkedManagerBase<QuestManager>.instance.ClientEvent_OnQuestStarted += new Action<DewQuest>(ClientEventOnQuestStarted);
	}

	private void ClientEventOnQuestStarted(DewQuest obj)
	{
		global::UnityEngine.Object.Instantiate(itemPrefab, itemParent).Setup(obj);
	}
}
