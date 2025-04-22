using System;
using UnityEngine;

public class UI_Lobby_EmoteCanvas : MonoBehaviour
{
	public Transform[] emotePivots;

	private void Start()
	{
		NetworkedManagerBase<ChatManager>.instance.ClientEvent_OnEmoteReceived += new Action<DewPlayer, string>(ClientEventOnEmoteReceived);
	}

	private void OnDestroy()
	{
		if (NetworkedManagerBase<ChatManager>.instance != null)
		{
			NetworkedManagerBase<ChatManager>.instance.ClientEvent_OnEmoteReceived -= new Action<DewPlayer, string>(ClientEventOnEmoteReceived);
		}
	}

	private void ClientEventOnEmoteReceived(DewPlayer sender, string emoteName)
	{
		Emote prefab = DewResources.GetByName<Emote>(emoteName);
		if (!(prefab == null))
		{
			global::UnityEngine.Object.Instantiate(prefab, base.transform).posGetter = () => emotePivots[DewPlayer.humanPlayers.IndexOf(sender)].position;
		}
	}
}
