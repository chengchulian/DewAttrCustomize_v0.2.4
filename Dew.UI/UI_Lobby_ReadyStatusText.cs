using TMPro;

[LogicUpdatePriority(1050)]
public class UI_Lobby_ReadyStatusText : LogicBehaviour
{
	private TextMeshProUGUI _text;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (!(NetworkedManagerBase<PlayLobbyManager>.softInstance == null))
		{
			int val = NetworkedManagerBase<PlayLobbyManager>.softInstance.numOfReadyPlayers;
			int max = NetworkedManagerBase<PlayLobbyManager>.softInstance.numOfReadyPlayersMax;
			if (val != max)
			{
				_text.text = string.Format(DewLocalization.GetUIValue("Lobby_ReadyStatus"), val, max);
			}
			else
			{
				_text.text = null;
			}
		}
	}
}
