using Mirror;
using UnityEngine.UI;

[LogicUpdatePriority(1050)]
public class UI_Lobby_PlayerListItem : UI_Lobby_PlayerInfoBase
{
	public UI_Toggle muteToggle;

	public Button kickButton;

	protected override void Start()
	{
		base.Start();
		muteToggle.onIsCheckedChanged.AddListener(MuteChanged);
		kickButton.onClick.AddListener(delegate
		{
			DewPlayer target = null;
			if (index < DewPlayer.humanPlayers.Count)
			{
				target = DewPlayer.humanPlayers[index];
			}
			if (!(target == null))
			{
				ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
				{
					owner = this,
					buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
					defaultButton = DewMessageSettings.ButtonType.Cancel,
					validator = () => target != null,
					destructiveConfirm = true,
					rawContent = string.Format(DewLocalization.GetUIValue("Message_KickPlayerConfirmation"), ChatManager.GetColoredDescribedPlayerName(target)),
					onClose = delegate(DewMessageSettings.ButtonType b)
					{
						if (b == DewMessageSettings.ButtonType.Yes && !(target == null) && NetworkServer.active)
						{
							target.Kick();
						}
					}
				});
			}
		});
	}

	private void MuteChanged(bool arg0)
	{
		if (arg0)
		{
			NetworkedManagerBase<ChatManager>.instance.MutePlayer(DewPlayer.humanPlayers[index]);
		}
		if (!arg0)
		{
			NetworkedManagerBase<ChatManager>.instance.UnmutePlayer(DewPlayer.humanPlayers[index]);
		}
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		DewPlayer target = DewPlayer.humanPlayers.GetOrDefault(index);
		muteToggle.gameObject.SetActive(target != null && !target.isOwned);
		kickButton.gameObject.SetActive(NetworkServer.active && target != null && !target.isOwned);
	}
}
