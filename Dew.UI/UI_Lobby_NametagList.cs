using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Lobby_NametagList : UI_NametagList
{
	public GameObject fxNametagChange;

	public GameObject newObject;

	private List<UI_Lobby_PlayerNametag> _players;

	private bool _didSetup;

	public new static UI_Lobby_NametagList instance => SingletonBehaviour<UI_NametagList>.instance as UI_Lobby_NametagList;

	protected override void Start()
	{
		base.Start();
		_players = global::UnityEngine.Object.FindObjectsOfType<UI_Lobby_PlayerNametag>().ToList();
		base.gameObject.SetActive(value: false);
	}

	private void OnEnable()
	{
		if (!_didSetup)
		{
			StartCoroutine(Routine());
		}
		IEnumerator Routine()
		{
			while (DewPlayer.local == null)
			{
				yield return null;
			}
			DewPlayer.local.ClientEvent_OnEquippedNametagChanged += new Action<string, string>(ClientEventOnNametagChanged);
			_didSetup = true;
		}
	}

	private void Update()
	{
		if (DewInput.GetButtonDown(MouseButton.Right, checkGameArea: false))
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void ClientEventOnNametagChanged(string arg1, string arg2)
	{
		UpdateSelected();
	}

	public override void RefreshList()
	{
		base.RefreshList();
		foreach (UI_NametagList_Item shownItem in shownItems)
		{
			UI_Toggle toggle = shownItem.GetComponent<UI_Toggle>();
			int index = toggle.index;
			toggle.onClick.AddListener(delegate
			{
				if (DewPlayer.local.equippedNametag == shownItems[index].nametagName)
				{
					DewPlayer.local.CmdSetNametag(null);
					DewEffect.Play(fxNametagChange);
				}
				else
				{
					DewPlayer.local.CmdSetNametag(shownItems[index].nametagName);
					DewEffect.Play(fxNametagChange);
				}
			});
		}
		UpdateSelected();
		UpdateNewStatus();
	}

	private void UpdateSelected()
	{
		if (DewPlayer.local == null)
		{
			return;
		}
		foreach (UI_NametagList_Item item in shownItems)
		{
			item.GetComponent<UI_Toggle>().isChecked = DewPlayer.local.equippedNametag == item.nametagName;
		}
	}

	public void UpdateNewStatus()
	{
		if (newObject == null)
		{
			return;
		}
		newObject.SetActive(value: false);
		foreach (KeyValuePair<string, DewProfile.CosmeticsData> nametag in DewSave.profile.nametags)
		{
			if (nametag.Value.isNew)
			{
				newObject.SetActive(value: true);
			}
		}
	}
}
