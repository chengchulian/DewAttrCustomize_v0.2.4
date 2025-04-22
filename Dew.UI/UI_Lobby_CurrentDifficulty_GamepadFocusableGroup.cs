using System.Linq;
using UnityEngine;

public class UI_Lobby_CurrentDifficulty_GamepadFocusableGroup : UI_GamepadFocusableGroup
{
	public override IGamepadFocusable GetEnterFocusable(Vector2 direction)
	{
		return GetComponentsInChildren<UI_DifficultyIcon>().ToList().Find((UI_DifficultyIcon icon) => icon.type == NetworkedManagerBase<GameSettingsManager>.instance.difficulty).GetComponentInParent<IGamepadFocusable>();
	}
}
