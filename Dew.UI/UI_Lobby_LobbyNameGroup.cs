using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_LobbyNameGroup : UI_Lobby_LobbyInfoBase
{
	public Button renameLobbyButton;

	public TextMeshProUGUI nameText;

	public GameObject nonEditGroup;

	public GameObject editGroup;

	public TMP_InputField inputField;

	public Button editConfirmButton;

	public Button editCancelButton;

	protected override void Start()
	{
		base.Start();
		renameLobbyButton.onClick.AddListener(OnEditStart);
		editConfirmButton.onClick.AddListener(OnEditConfirm);
		editCancelButton.onClick.AddListener(OnEditCancel);
		nonEditGroup.SetActive(value: true);
		editGroup.SetActive(value: false);
		inputField.onSubmit.AddListener(delegate
		{
			OnEditConfirm();
		});
	}

	private void OnEditStart()
	{
		if (!editGroup.activeInHierarchy && ManagerBase<LobbyManager>.instance.isLobbyLeader)
		{
			nonEditGroup.SetActive(value: false);
			editGroup.SetActive(value: true);
			inputField.text = nameText.text;
			inputField.caretPosition = inputField.text.Length;
			if (DewInput.currentMode == InputMode.Gamepad)
			{
				inputField.GetComponent<UI_GamepadInputField>().StartInput(OnEditConfirm, OnEditCancel);
			}
		}
	}

	private void OnEditCancel()
	{
		if (editGroup.activeInHierarchy)
		{
			nonEditGroup.SetActive(value: true);
			editGroup.SetActive(value: false);
			if (DewInput.currentMode == InputMode.Gamepad)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(renameLobbyButton.GetComponent<IGamepadFocusable>());
			}
		}
	}

	private void OnEditConfirm()
	{
		if (editGroup.activeInHierarchy)
		{
			nonEditGroup.SetActive(value: true);
			editGroup.SetActive(value: false);
			if (inputField.text.Length == 0)
			{
				inputField.text = LobbyManager.GetDefaultLobbyName();
			}
			ManagerBase<LobbyManager>.instance.service.SetLobbyName(inputField.text);
			nameText.text = inputField.text;
			DewSave.profile.preferredLobbyName = inputField.text;
			if (DewInput.currentMode == InputMode.Gamepad)
			{
				ManagerBase<GlobalUIManager>.instance.SetFocus(renameLobbyButton.GetComponent<IGamepadFocusable>());
			}
		}
	}

	public override void OnLobbyUpdated()
	{
		if (ManagerBase<LobbyManager>.instance.service.currentLobby == null || ManagerBase<LobbyManager>.instance.service.currentLobby.isInviteOnly)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		renameLobbyButton.gameObject.SetActive(ManagerBase<LobbyManager>.instance.service.currentLobby.isLobbyLeader);
		nameText.text = ManagerBase<LobbyManager>.instance.service.currentLobby.name;
	}
}
