using System;
using UnityEngine;

public class UI_Lobby_StartButtonDejavuCost : MonoBehaviour
{
	private void Start()
	{
		Dew.CallOnReady(this, () => DewPlayer.local != null, delegate
		{
			DewPlayer.local.ClientEvent_OnSelectedDejavuItemChanged += new Action<string>(ClientEventOnSelectedDejavuItemChanged);
			ClientEventOnSelectedDejavuItemChanged(null);
		});
	}

	private void OnDestroy()
	{
		if (DewPlayer.local != null)
		{
			DewPlayer.local.ClientEvent_OnSelectedDejavuItemChanged -= new Action<string>(ClientEventOnSelectedDejavuItemChanged);
		}
	}

	private void ClientEventOnSelectedDejavuItemChanged(string obj)
	{
		if (string.IsNullOrEmpty(DewPlayer.local.selectedDejavuItem))
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		GetComponentInChildren<CostDisplay>().Setup(new Cost
		{
			stardust = NetworkedManagerBase<GameSettingsManager>.instance.localPlayerDejavuCost
		});
	}
}
