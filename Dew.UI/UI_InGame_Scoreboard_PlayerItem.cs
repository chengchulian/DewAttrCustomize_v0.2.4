using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[LogicUpdatePriority(3000)]
public class UI_InGame_Scoreboard_PlayerItem : LogicBehaviour
{
	public TextMeshProUGUI playerNameText;

	public TextMeshProUGUI heroNameText;

	public UI_HeroIcon icon;

	public RawImage steamAvatar;

	public UI_Toggle muteToggle;

	public Button kickButton;

	public Button shareCurrencyButton;

	public DewPlayer player;

	public Hero hero;

	private UI_EntityProvider _entityProvider;

	private void Start()
	{
		if (hero.IsNullOrInactive())
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		muteToggle.onIsCheckedChanged.AddListener(MuteChanged);
		muteToggle.isChecked = NetworkedManagerBase<ChatManager>.instance.IsPlayerMuted(hero.owner);
		kickButton.onClick.AddListener(Kick);
		shareCurrencyButton.onClick.AddListener(GiveCurrency);
		DewNetworkManager instance = DewNetworkManager.instance;
		instance.onHumanPlayerRemove = (Action<DewPlayer>)Delegate.Combine(instance.onHumanPlayerRemove, new Action<DewPlayer>(OnHumanPlayerRemove));
		hero.ClientActorEvent_OnDestroyed += new Action<Actor>(ClientActorEventOnDestroyed);
	}

	private void ClientActorEventOnDestroyed(Actor obj)
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (DewNetworkManager.instance != null)
		{
			DewNetworkManager instance = DewNetworkManager.instance;
			instance.onHumanPlayerRemove = (Action<DewPlayer>)Delegate.Remove(instance.onHumanPlayerRemove, new Action<DewPlayer>(OnHumanPlayerRemove));
		}
	}

	private void OnHumanPlayerRemove(DewPlayer obj)
	{
		if (player == obj)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void GiveCurrency()
	{
		InGameUIManager.instance.isScoreboardDisplayed = false;
		UI_InGame_GiveCurrencyView uI_InGame_GiveCurrencyView = global::UnityEngine.Object.FindObjectOfType<UI_InGame_GiveCurrencyView>(includeInactive: true);
		uI_InGame_GiveCurrencyView.giveTarget = hero.owner;
		uI_InGame_GiveCurrencyView.Show();
	}

	private void Kick()
	{
		InGameUIManager.instance.isScoreboardDisplayed = false;
		ManagerBase<MessageManager>.instance.ShowMessage(new DewMessageSettings
		{
			owner = this,
			buttons = (DewMessageSettings.ButtonType.Yes | DewMessageSettings.ButtonType.Cancel),
			defaultButton = DewMessageSettings.ButtonType.Cancel,
			validator = () => hero != null && hero.owner != null,
			destructiveConfirm = true,
			rawContent = string.Format(DewLocalization.GetUIValue("Message_KickPlayerConfirmation"), ChatManager.GetColoredDescribedPlayerName(hero.owner)),
			onClose = delegate(DewMessageSettings.ButtonType b)
			{
				if (b == DewMessageSettings.ButtonType.Yes && !(hero == null) && !(hero.owner == null) && NetworkServer.active)
				{
					hero.owner.Kick();
				}
			}
		});
	}

	private void MuteChanged(bool arg0)
	{
		if (arg0)
		{
			NetworkedManagerBase<ChatManager>.instance.MutePlayer(hero.owner);
		}
		if (!arg0)
		{
			NetworkedManagerBase<ChatManager>.instance.UnmutePlayer(hero.owner);
		}
	}

	public void Setup(Hero newHero)
	{
		player = newHero.owner;
		hero = newHero;
		steamAvatar.texture = null;
		playerNameText.text = newHero.owner.playerName;
		heroNameText.text = DewLocalization.GetUIValue(newHero.GetType().Name + "_Name");
		icon.Setup(newHero.GetType().Name);
		steamAvatar.texture = newHero.owner.avatar;
		steamAvatar.gameObject.SetActive(steamAvatar.texture != null);
		muteToggle.gameObject.SetActive(!hero.isOwned);
		kickButton.gameObject.SetActive(NetworkServer.active && !hero.isOwned);
		shareCurrencyButton.gameObject.SetActive(!hero.isOwned);
	}
}
