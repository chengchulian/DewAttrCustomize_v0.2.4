using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_LobbyID : UI_Lobby_LobbyInfoBase
{
	public TextMeshProUGUI idText;

	public Button toggleButton;

	public GameObject isHiddenObject;

	public GameObject isShownObject;

	private bool _isHidden = true;

	protected override void Start()
	{
		base.Start();
		toggleButton.onClick.AddListener(Toggle);
		_isHidden = true;
	}

	public override void OnLobbyUpdated()
	{
		UpdateText();
	}

	private void Toggle()
	{
		_isHidden = !_isHidden;
		UpdateText();
	}

	private void UpdateText()
	{
		if (ManagerBase<LobbyManager>.instance == null || ManagerBase<LobbyManager>.instance.service.currentLobby == null)
		{
			idText.text = "";
			isHiddenObject.SetActive(value: false);
			isShownObject.SetActive(value: false);
			toggleButton.gameObject.SetActive(value: false);
			return;
		}
		toggleButton.gameObject.SetActive(value: true);
		isHiddenObject.SetActive(_isHidden);
		isShownObject.SetActive(!_isHidden);
		string id = ManagerBase<LobbyManager>.instance.service.currentLobby.shortCode;
		if (id == null)
		{
			idText.text = "-";
			return;
		}
		if (id.Length < 6)
		{
			idText.text = id.ToUpperInvariant();
			return;
		}
		int halfLen = id.Length / 2;
		string t = id.Substring(0, halfLen).ToUpperInvariant() + " " + id.Substring(halfLen).ToUpperInvariant();
		if (_isHidden)
		{
			idText.text = "";
			for (int i = 0; i < t.Length; i++)
			{
				if (t[i] != ' ')
				{
					idText.text += "*";
				}
				else
				{
					idText.text += " ";
				}
			}
		}
		else
		{
			idText.text = t;
		}
	}

	public void CopyToClipboard()
	{
		string id = ManagerBase<LobbyManager>.instance.service.currentLobby.shortCode;
		if (id.Length < 6)
		{
			GUIUtility.systemCopyBuffer = id.ToUpperInvariant();
			return;
		}
		int halfLen = id.Length / 2;
		GUIUtility.systemCopyBuffer = id.Substring(0, halfLen).ToUpperInvariant() + " " + id.Substring(halfLen).ToUpperInvariant();
	}
}
