using Mirror;
using TMPro;
using UnityEngine.UI;

[LogicUpdatePriority(1050)]
public class UI_Lobby_StartButtonController : LogicBehaviour
{
	public Button startButton;

	public UI_Toggle readyToggle;

	public TextMeshProUGUI readyText;

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		if (NetworkServer.active)
		{
			startButton.gameObject.SetActive(value: true);
			readyToggle.gameObject.SetActive(value: false);
			startButton.interactable = NetworkedManagerBase<PlayLobbyManager>.instance.isEveryoneReady && !NetworkedManagerBase<PlayLobbyManager>.instance.hasStarted;
		}
		else if (NetworkClient.isConnected && DewPlayer.local != null)
		{
			startButton.gameObject.SetActive(value: false);
			readyToggle.gameObject.SetActive(value: true);
			readyToggle.isChecked = DewPlayer.local.isReady;
			readyText.text = DewLocalization.GetUIValue(DewPlayer.local.isReady ? "Lobby_CancelReady" : "Lobby_Ready");
		}
		else
		{
			startButton.gameObject.SetActive(value: false);
			readyToggle.gameObject.SetActive(value: false);
		}
	}
}
