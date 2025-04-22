using TMPro;
using UnityEngine;

public class UI_Lobby_LobbyType : UI_Lobby_LobbyInfoBase
{
	public TextMeshProUGUI typeText;

	private CanvasGroup _cg;

	protected override void Start()
	{
		base.Start();
		_cg = GetComponent<CanvasGroup>();
	}

	public override void OnLobbyUpdated()
	{
		if (!(ManagerBase<LobbyManager>.instance != null) || ManagerBase<LobbyManager>.instance.service.currentLobby != null)
		{
			LobbyType type = (ManagerBase<LobbyManager>.instance.service.currentLobby.isInviteOnly ? LobbyType.InviteOnly : LobbyType.Public);
			typeText.text = DewLocalization.GetUIValue($"Lobby_Type_{type}");
			if (!(_cg == null))
			{
				_cg.interactable = true;
				_cg.blocksRaycasts = true;
				_cg.alpha = 1f;
			}
		}
	}
}
