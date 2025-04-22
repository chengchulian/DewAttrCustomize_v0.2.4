using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_DejavuButton : MonoBehaviour
{
	public GameObject activeObject;

	public GameObject lockedObject;

	public UI_Achievement_Icon icon;

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
		bool isUnlocked = DewSave.profile.heroMasteries.Any((KeyValuePair<string, DewProfile.HeroMasteryData> m) => m.Value.currentLevel >= 30);
		lockedObject.SetActive(!isUnlocked);
		Dew.CallOnReady(this, () => DewPlayer.local != null, delegate
		{
			DewPlayer.local.ClientEvent_OnSelectedDejavuItemChanged += new Action<string>(ClientEventOnSelectedDejavuItemChanged);
			UpdateActiveObject();
		});
	}

	private void OnDestroy()
	{
		if (DewPlayer.local != null)
		{
			DewPlayer.local.ClientEvent_OnSelectedDejavuItemChanged -= new Action<string>(ClientEventOnSelectedDejavuItemChanged);
		}
	}

	private void ClientEventOnSelectedDejavuItemChanged(string obj)
	{
		UpdateActiveObject();
	}

	private void OnClick()
	{
		if (lockedObject.activeSelf)
		{
			ManagerBase<MessageManager>.instance.ShowMessageLocalized("Dejavu_LockedRequirementMessage");
		}
		else
		{
			LobbyUIManager.instance.SetState("Dejavu");
		}
	}

	private void UpdateActiveObject()
	{
		activeObject.SetActive(!string.IsNullOrEmpty(DewPlayer.local.selectedDejavuItem));
		if (activeObject.activeSelf)
		{
			icon.SetupByItem(DewResources.GetByShortTypeName(DewPlayer.local.selectedDejavuItem));
		}
	}
}
