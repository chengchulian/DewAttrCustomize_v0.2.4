using UnityEngine;

public class UI_Lobby_Emotes_Back : MonoBehaviour
{
	private void Update()
	{
		if (DewInput.GetButtonDown(MouseButton.Right, checkGameArea: false))
		{
			GoBackWithConfirmation();
		}
	}

	public void GoBackWithConfirmation()
	{
		if (SingletonBehaviour<UI_EmoteList_EditEmote>.instance.dirtyObject.activeSelf)
		{
			ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
			{
				owner = this,
				buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.No),
				defaultButton = DewMessageSettings.ButtonType.No,
				destructiveConfirm = true,
				validator = () => LobbyUIManager.softInstance != null && LobbyUIManager.softInstance.IsState("Emotes"),
				rawContent = DewLocalization.GetUIValue("Settings_ConfirmUnsavedChanges"),
				onClose = delegate(DewMessageSettings.ButtonType b)
				{
					if (b == DewMessageSettings.ButtonType.Yes)
					{
						SingletonBehaviour<UI_EmoteList_EditEmote>.instance.Revert();
						LobbyUIManager.instance.SetState("Lobby");
					}
				}
			});
		}
		else
		{
			LobbyUIManager.instance.SetState("Lobby");
		}
	}
}
