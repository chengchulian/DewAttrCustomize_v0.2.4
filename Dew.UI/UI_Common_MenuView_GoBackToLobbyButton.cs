using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UI_Common_MenuView_GoBackToLobbyButton : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(delegate
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				rawContent = DewLocalization.GetUIValue("Menu_Message_ReturnToLobbyConfirm"),
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.No),
				defaultButton = DewMessageSettings.ButtonType.No,
				destructiveConfirm = true,
				onClose = delegate(DewMessageSettings.ButtonType button)
				{
					if (button == DewMessageSettings.ButtonType.Yes && !(DewNetworkManager.instance == null) && NetworkServer.active && !(NetworkedManagerBase<GameManager>.instance == null))
					{
						DewNetworkManager.instance.RestartSession();
					}
				}
			});
		});
		base.gameObject.SetActive(NetworkServer.active && NetworkedManagerBase<GameManager>.instance != null);
	}
}
