using System;
using TMPro;
using UnityEngine;

public class UI_Lobby_PlayerNametag : UI_Lobby_PlayerInfoBase
{
	public RectTransform nametagContainer;

	public PlayLobby_Character followTarget;

	public Vector3 followWorldOffset;

	public TextMeshProUGUI playerHeroType;

	public bool doSubtitle;

	private Nametag _currentNametag;

	protected override void Start()
	{
		base.Start();
		DewNetworkManager instance = DewNetworkManager.instance;
		instance.onHumanPlayerAdd = (Action<DewPlayer>)Delegate.Combine(instance.onHumanPlayerAdd, new Action<DewPlayer>(OnHumanPlayerAdd));
		DewNetworkManager instance2 = DewNetworkManager.instance;
		instance2.onHumanPlayerRemove = (Action<DewPlayer>)Delegate.Combine(instance2.onHumanPlayerRemove, new Action<DewPlayer>(OnHumanPlayerRemove));
		foreach (DewPlayer h in DewPlayer.humanPlayers)
		{
			OnHumanPlayerAdd(h);
		}
	}

	private void LateUpdate()
	{
		base.transform.position = Dew.mainCamera.WorldToScreenPoint(followTarget.model.GetAbovePosition() + followWorldOffset).Quantitized();
		Quaternion rot = Quaternion.Inverse(Dew.mainCamera.transform.rotation) * followTarget.transform.rotation;
		rot *= Quaternion.Euler(0f, 180f, 0f);
		base.transform.rotation = Quaternion.Lerp(rot, Quaternion.identity, 0.5f);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		DewPlayer target = DewPlayer.humanPlayers.GetOrDefault(index);
		if (!(target == null))
		{
			if (doSubtitle)
			{
				playerHeroType.text = DewLocalization.GetUIValue(target.selectedHeroType + "_Name") + ", " + DewLocalization.GetUIValue(target.selectedHeroType + "_Subtitle");
			}
			else
			{
				playerHeroType.text = DewLocalization.GetUIValue(target.selectedHeroType + "_Name");
			}
		}
	}

	private void OnDestroy()
	{
		if (DewNetworkManager.instance != null)
		{
			DewNetworkManager instance = DewNetworkManager.instance;
			instance.onHumanPlayerAdd = (Action<DewPlayer>)Delegate.Remove(instance.onHumanPlayerAdd, new Action<DewPlayer>(OnHumanPlayerAdd));
			DewNetworkManager instance2 = DewNetworkManager.instance;
			instance2.onHumanPlayerRemove = (Action<DewPlayer>)Delegate.Remove(instance2.onHumanPlayerRemove, new Action<DewPlayer>(OnHumanPlayerRemove));
		}
		foreach (DewPlayer h in DewPlayer.humanPlayers)
		{
			OnHumanPlayerRemove(h);
		}
	}

	private void OnHumanPlayerAdd(DewPlayer obj)
	{
		TryRefreshNametag();
		obj.ClientEvent_OnEquippedNametagChanged += new Action<string, string>(ClientEventOnEquippedNametagChanged);
	}

	private void OnHumanPlayerRemove(DewPlayer obj)
	{
		TryRefreshNametag();
		if (!(obj == null))
		{
			obj.ClientEvent_OnEquippedNametagChanged -= new Action<string, string>(ClientEventOnEquippedNametagChanged);
		}
	}

	private void ClientEventOnEquippedNametagChanged(string from, string to)
	{
		TryRefreshNametag();
	}

	private void TryRefreshNametag()
	{
		DewPlayer p = DewPlayer.humanPlayers.GetOrDefault(index);
		string targetNametag = null;
		if (p != null && p.IsAllowedToUseItem(p.equippedNametag))
		{
			targetNametag = p.equippedNametag;
		}
		if ((_currentNametag == null && string.IsNullOrEmpty(targetNametag)) || (_currentNametag != null && _currentNametag.name == targetNametag))
		{
			return;
		}
		if (_currentNametag != null)
		{
			global::UnityEngine.Object.Destroy(_currentNametag.gameObject);
			_currentNametag = null;
		}
		if (!string.IsNullOrEmpty(targetNametag))
		{
			Nametag prefab = DewResources.GetByName<Nametag>(targetNametag);
			if (!(prefab == null))
			{
				_currentNametag = global::UnityEngine.Object.Instantiate(prefab, nametagContainer);
				_currentNametag.Setup(nametagContainer, isIcon: false);
				_currentNametag.name = targetNametag;
			}
		}
	}
}
