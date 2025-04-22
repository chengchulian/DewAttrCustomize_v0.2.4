using System;
using UnityEngine;

public class UI_InGame_Difficulty : MonoBehaviour
{
	private UI_DifficultyIcon _icon;

	private void Start()
	{
		_icon = GetComponentInChildren<UI_DifficultyIcon>();
		NetworkedManagerBase<GameManager>.instance.ClientEvent_OnDifficultyChanged += new Action<DewDifficultySettings, DewDifficultySettings>(ClientEventOnDifficultyChanged);
		ClientEventOnDifficultyChanged(null, NetworkedManagerBase<GameManager>.instance.difficulty);
	}

	private void ClientEventOnDifficultyChanged(DewDifficultySettings arg1, DewDifficultySettings arg2)
	{
		if (!(arg2 == null))
		{
			_icon.Setup(arg2.name);
		}
	}
}
