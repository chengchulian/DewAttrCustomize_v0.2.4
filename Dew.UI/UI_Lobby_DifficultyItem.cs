using System;
using Mirror;
using UnityEngine;

public class UI_Lobby_DifficultyItem : LogicBehaviour
{
	public UI_DifficultyIcon icon;

	public GameObject isSelectedObject;

	private void Start()
	{
		GameSettingsManager instance = NetworkedManagerBase<GameSettingsManager>.instance;
		instance.onDifficultyChanged = (Action<string, string>)Delegate.Combine(instance.onDifficultyChanged, new Action<string, string>(OnDifficultyChanged));
		OnDifficultyChanged(null, NetworkedManagerBase<GameSettingsManager>.instance.difficulty);
	}

	private void OnDifficultyChanged(string arg1, string arg2)
	{
		isSelectedObject.SetActive(icon.type == arg2);
		icon.GetComponent<CanvasGroup>().alpha = ((icon.type == arg2) ? 0.2f : 1f);
	}

	public void Click()
	{
		if (NetworkServer.active)
		{
			NetworkedManagerBase<GameSettingsManager>.instance.SetDifficulty(icon.type);
		}
	}
}
