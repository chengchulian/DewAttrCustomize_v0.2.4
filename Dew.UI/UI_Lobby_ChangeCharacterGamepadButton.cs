using UnityEngine;

public class UI_Lobby_ChangeCharacterGamepadButton : UI_GamepadFocusable, IGamepadFocusableOverrideInput
{
	private PlayLobby_Character[] _lobbyCharacters;

	private void Start()
	{
		_lobbyCharacters = Object.FindObjectsOfType<PlayLobby_Character>();
	}

	public bool OnGamepadConfirm()
	{
		PlayLobby_Character[] lobbyCharacters = _lobbyCharacters;
		foreach (PlayLobby_Character l in lobbyCharacters)
		{
			if (l.IsLocalPlayer())
			{
				l.Click();
				break;
			}
		}
		return true;
	}

	public override bool CanBeFocused()
	{
		if (base.CanBeFocused())
		{
			return LobbyUIManager.instance.IsState("Lobby");
		}
		return false;
	}
}
