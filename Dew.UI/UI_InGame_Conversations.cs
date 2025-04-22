using System;
using UnityEngine;

public class UI_InGame_Conversations : MonoBehaviour
{
	public Transform parent;

	public UI_InGame_Conversations_Instance instancePrefab;

	private void Start()
	{
		NetworkedManagerBase<ConversationManager>.instance.ClientEvent_OnStartConversation += new Action<uint>(ClientEventOnStartConversation);
	}

	private void ClientEventOnStartConversation(uint obj)
	{
		if (NetworkedManagerBase<ConversationManager>.instance.convSettings[obj].isVisibleToLocal)
		{
			global::UnityEngine.Object.Instantiate(instancePrefab, parent).Setup(obj);
		}
	}
}
