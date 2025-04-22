using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_LucidDreams_Item : MonoBehaviour
{
	public GameObject activeObject;

	private UI_LucidDreamIcon _icon;

	private string _type;

	public void Setup(string type)
	{
		_type = type;
		_icon = GetComponentInChildren<UI_LucidDreamIcon>();
		_icon.Setup(type);
		UpdateEnabledState();
		GetComponentInChildren<Button>().onClick.AddListener(Click);
		GameSettingsManager instance = NetworkedManagerBase<GameSettingsManager>.instance;
		instance.onLucidDreamsChanged = (Action)Delegate.Combine(instance.onLucidDreamsChanged, new Action(UpdateEnabledState));
		NetworkedManagerBase<PlayLobbyManager>.instance.ClientEvent_OnAvailableLucidDreamsChanged += new Action(UpdateUnlockedState);
		UpdateUnlockedState();
	}

	private void OnDestroy()
	{
		if (NetworkedManagerBase<GameSettingsManager>.instance != null)
		{
			GameSettingsManager instance = NetworkedManagerBase<GameSettingsManager>.instance;
			instance.onLucidDreamsChanged = (Action)Delegate.Remove(instance.onLucidDreamsChanged, new Action(UpdateEnabledState));
		}
		if (NetworkedManagerBase<PlayLobbyManager>.instance != null)
		{
			NetworkedManagerBase<PlayLobbyManager>.instance.ClientEvent_OnAvailableLucidDreamsChanged -= new Action(UpdateUnlockedState);
		}
	}

	private void UpdateEnabledState()
	{
		bool isActive = NetworkedManagerBase<GameSettingsManager>.instance.lucidDreams.Contains(_type);
		activeObject.SetActive(isActive);
		_icon.GetComponent<CanvasGroup>().alpha = (isActive ? 0.35f : 1f);
	}

	private void UpdateUnlockedState()
	{
		bool isUnlocked = NetworkedManagerBase<PlayLobbyManager>.instance.availableLucidDreams.Contains(_type);
		if (NetworkServer.active && !isUnlocked && NetworkedManagerBase<GameSettingsManager>.instance.lucidDreams.Contains(_type))
		{
			NetworkedManagerBase<GameSettingsManager>.instance.RemoveLucidDream(_type);
		}
		_icon.ignoreAchievement = isUnlocked;
		if (isUnlocked)
		{
			_icon.ShowColor();
		}
		else
		{
			_icon.ShowLocked();
		}
	}

	public void Click()
	{
		if (NetworkServer.active && NetworkedManagerBase<PlayLobbyManager>.instance.availableLucidDreams.Contains(_type))
		{
			if (NetworkedManagerBase<GameSettingsManager>.instance.lucidDreams.Contains(_type))
			{
				NetworkedManagerBase<GameSettingsManager>.instance.RemoveLucidDream(_type);
			}
			else
			{
				NetworkedManagerBase<GameSettingsManager>.instance.AddLucidDream(_type);
			}
		}
	}
}
