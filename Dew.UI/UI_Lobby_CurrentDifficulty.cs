using System;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

[LogicUpdatePriority(1050)]
public class UI_Lobby_CurrentDifficulty : LogicBehaviour
{
	public TextMeshProUGUI dropdownText;

	public TextMeshProUGUI descText;

	public Color descMultiplyColor;

	public Color descAddColor;

	public TMP_Dropdown dropdown;

	public List<string> options;

	private void Awake()
	{
		List<string> list = new List<string>();
		dropdown.ClearOptions();
		foreach (string o in options)
		{
			list.Add(DewLocalization.GetUIValue("Difficulty_" + o + "_Name"));
		}
		dropdown.AddOptions(list);
		dropdown.onValueChanged.AddListener(delegate(int index)
		{
			if (NetworkServer.active)
			{
				NetworkedManagerBase<GameSettingsManager>.instance.SetDifficulty(options[index]);
			}
		});
	}

	private void Start()
	{
		GameSettingsManager instance = NetworkedManagerBase<GameSettingsManager>.instance;
		instance.onDifficultyChanged = (Action<string, string>)Delegate.Combine(instance.onDifficultyChanged, new Action<string, string>(OnDifficultyChanged));
		OnDifficultyChanged(null, NetworkedManagerBase<GameSettingsManager>.instance.difficulty);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		dropdown.interactable = NetworkServer.active;
	}

	private void OnDifficultyChanged(string arg1, string arg2)
	{
		DewDifficultySettings diff = DewResources.GetByName<DewDifficultySettings>(arg2);
		dropdown.value = options.FindIndex((string s) => s == arg2);
		dropdownText.color = diff.difficultyColor;
		descText.text = DewLocalization.GetUIValue("Difficulty_" + arg2 + "_Description");
		descText.color = diff.difficultyColor * descMultiplyColor + descAddColor;
	}
}
