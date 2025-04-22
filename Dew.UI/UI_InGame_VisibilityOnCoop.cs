using System;
using UnityEngine;

public class UI_InGame_VisibilityOnCoop : MonoBehaviour
{
	public bool showOnCoop;

	private void Start()
	{
		DewNetworkManager instance = DewNetworkManager.instance;
		instance.onHumanPlayerAdd = (Action<DewPlayer>)Delegate.Combine(instance.onHumanPlayerAdd, new Action<DewPlayer>(UpdateVisibility));
		DewNetworkManager instance2 = DewNetworkManager.instance;
		instance2.onHumanPlayerRemove = (Action<DewPlayer>)Delegate.Combine(instance2.onHumanPlayerRemove, new Action<DewPlayer>(UpdateVisibility));
		UpdateVisibility(null);
	}

	private void OnDestroy()
	{
		if (DewNetworkManager.instance != null)
		{
			DewNetworkManager instance = DewNetworkManager.instance;
			instance.onHumanPlayerAdd = (Action<DewPlayer>)Delegate.Remove(instance.onHumanPlayerAdd, new Action<DewPlayer>(UpdateVisibility));
			DewNetworkManager instance2 = DewNetworkManager.instance;
			instance2.onHumanPlayerRemove = (Action<DewPlayer>)Delegate.Remove(instance2.onHumanPlayerRemove, new Action<DewPlayer>(UpdateVisibility));
		}
	}

	private void UpdateVisibility(DewPlayer _)
	{
		base.gameObject.SetActive(DewPlayer.humanPlayers.Count > 1 == showOnCoop);
	}
}
