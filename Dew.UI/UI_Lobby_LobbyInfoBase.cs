using System;
using UnityEngine;

public abstract class UI_Lobby_LobbyInfoBase : MonoBehaviour
{
	protected virtual void Start()
	{
		LobbyManager instance = ManagerBase<LobbyManager>.instance;
		instance.onCurrentLobbyChanged = (Action)Delegate.Combine(instance.onCurrentLobbyChanged, new Action(OnLobbyUpdated));
		OnLobbyUpdated();
	}

	protected virtual void OnDestroy()
	{
		if (ManagerBase<LobbyManager>.instance != null)
		{
			LobbyManager instance = ManagerBase<LobbyManager>.instance;
			instance.onCurrentLobbyChanged = (Action)Delegate.Remove(instance.onCurrentLobbyChanged, new Action(OnLobbyUpdated));
		}
	}

	public abstract void OnLobbyUpdated();
}
