using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Title_FindLobbyView : View
{
	public UI_Title_FindLobby_LobbyItem itemPrefab;

	public Transform itemParent;

	public GameObject loadingObject;

	public GameObject emptyObject;

	public Button refreshButton;

	public Button joinButton;

	public UI_ToggleGroup itemGroup;

	public GameObject joinWithLobbyIDWindow;

	public TMP_InputField lobbyIdInputField;

	public Button lobbyJoinWithIdButton;

	public float refreshCooldownTime = 3f;

	private LobbySearchResult _currentLobbies;

	private float _lastRefreshTime = float.NegativeInfinity;

	protected override void Start()
	{
		base.Start();
		if (Application.IsPlaying(this))
		{
			refreshButton.onClick.AddListener(Refresh);
			joinButton.onClick.AddListener(Join);
			itemGroup.onCurrentIndexChanged.AddListener(SelectedLobbyChanged);
			lobbyIdInputField.onValueChanged.AddListener(OnLobbyIDChanged);
			lobbyIdInputField.onSubmit.AddListener(delegate
			{
				ConfirmJoinWithLobbyID();
			});
			LobbyManager instance = ManagerBase<LobbyManager>.instance;
			instance.onLobbyListUpdated = (Action)Delegate.Combine(instance.onLobbyListUpdated, new Action(OnLobbyListUpdated));
		}
	}

	private new void OnDestroy()
	{
		if (ManagerBase<LobbyManager>.instance != null)
		{
			LobbyManager instance = ManagerBase<LobbyManager>.instance;
			instance.onLobbyListUpdated = (Action)Delegate.Remove(instance.onLobbyListUpdated, new Action(OnLobbyListUpdated));
		}
	}

	private void SelectedLobbyChanged(int arg0)
	{
		bool isValid = arg0 >= 0 && arg0 < _currentLobbies.lobbies.Count;
		joinButton.interactable = isValid;
	}

	private void Join()
	{
		int i = itemGroup.currentIndex;
		if (i >= 0 && i < _currentLobbies.lobbies.Count)
		{
			LobbyInstance lobby = _currentLobbies.lobbies[i];
			ManagerBase<TitleManager>.instance.JoinLobby(lobby.id);
		}
	}

	protected override void OnShow()
	{
		base.OnShow();
		if (!ManagerBase<LobbyManager>.instance.service.isRefreshingLobby)
		{
			ClearItems();
			Refresh();
		}
	}

	public void Refresh()
	{
		if (!ManagerBase<LobbyManager>.instance.service.isRefreshingLobby)
		{
			itemGroup.currentIndex = -1;
			refreshButton.interactable = false;
			emptyObject.SetActive(value: false);
			ClearItems();
			ManagerBase<LobbyManager>.instance.service.RefreshLobbies();
			_lastRefreshTime = Time.unscaledTime;
		}
	}

	private void ClearItems()
	{
		for (int i = itemParent.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.Destroy(itemParent.GetChild(i).gameObject);
		}
	}

	private void OnLobbyListUpdated()
	{
		if (this == null)
		{
			return;
		}
		ClearItems();
		_lastRefreshTime = Time.unscaledTime;
		_currentLobbies = ManagerBase<LobbyManager>.instance.service.foundLobbies;
		if (_currentLobbies != null && _currentLobbies.lobbies.Count > 0)
		{
			itemGroup.currentIndex = 0;
		}
		if (_currentLobbies != null)
		{
			for (int i = 0; i < _currentLobbies.lobbies.Count; i++)
			{
				LobbyInstance l = _currentLobbies.lobbies[i];
				UI_Title_FindLobby_LobbyItem uI_Title_FindLobby_LobbyItem = global::UnityEngine.Object.Instantiate(itemPrefab, itemParent);
				uI_Title_FindLobby_LobbyItem.Setup(l, i);
				uI_Title_FindLobby_LobbyItem.onDoubleClick = (Action)Delegate.Combine(uI_Title_FindLobby_LobbyItem.onDoubleClick, new Action(Join));
			}
		}
	}

	public void ShowFindLobbyHelp()
	{
		ManagerBase<MessageManager>.instance.ShowMessageLocalized("Title_Message_FindLobbyHelp");
	}

	public void OpenJoinWithLobbyIDWindow()
	{
		joinWithLobbyIDWindow.SetActive(value: true);
		if (DewInput.currentMode == InputMode.KeyboardAndMouse)
		{
			string content = GUIUtility.systemCopyBuffer;
			if (IsLobbyIDInputValid(content))
			{
				lobbyIdInputField.text = content;
			}
		}
		else
		{
			lobbyIdInputField.text = "";
		}
		lobbyIdInputField.ActivateInputField();
	}

	public void CloseJoinWithLobbyIDWindow()
	{
		joinWithLobbyIDWindow.SetActive(value: false);
		lobbyIdInputField.text = "";
	}

	private void OnLobbyIDChanged(string arg0)
	{
		lobbyJoinWithIdButton.interactable = IsLobbyIDInputValid(arg0);
	}

	private bool IsLobbyIDInputValid(string input)
	{
		input = input.Trim();
		if (input.Length > 15)
		{
			return false;
		}
		if (input.Length <= 0)
		{
			return false;
		}
		return true;
	}

	public void ConfirmJoinWithLobbyID()
	{
		if (lobbyIdInputField.text.Length > 0)
		{
			string input = lobbyIdInputField.text.Trim();
			CloseJoinWithLobbyIDWindow();
			ManagerBase<TitleManager>.instance.JoinLobby(input);
		}
	}

	private void FixedUpdate()
	{
		if (base.isShowing)
		{
			refreshButton.interactable = !ManagerBase<LobbyManager>.instance.service.isRefreshingLobby && Time.unscaledTime - _lastRefreshTime > refreshCooldownTime;
			loadingObject.SetActive(ManagerBase<LobbyManager>.instance.service.isRefreshingLobby);
			emptyObject.SetActive(!ManagerBase<LobbyManager>.instance.service.isRefreshingLobby && (_currentLobbies == null || _currentLobbies.lobbies.Count == 0));
		}
	}
}
