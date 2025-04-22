using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Lobby_AccList : UI_AccList
{
	public GameObject fxNametagChange;

	protected override void Start()
	{
		base.Start();
		Dew.CallOnReady(this, () => DewPlayer.local != null, delegate
		{
			DewPlayer.local.ClientEvent_OnSelectedAccessoriesChanged += new Action(ClientEventOnSelectedAccessoriesChanged);
		});
	}

	private void ClientEventOnSelectedAccessoriesChanged()
	{
		UpdateSelected();
	}

	private void OnEnable()
	{
		RefreshList();
	}

	public override void RefreshList()
	{
		base.RefreshList();
		foreach (UI_AccList_Item item in shownItems)
		{
			UI_Toggle toggle = item.GetComponent<UI_Toggle>();
			int index = toggle.index;
			toggle.onIsCheckedChanged.AddListener(delegate
			{
				if (toggle.isChecked)
				{
					AddAcc(shownItems[index].accName);
				}
				else
				{
					RemoveAcc(shownItems[index].accName);
				}
			});
		}
		UpdateSelected();
	}

	private void AddAcc(string accName)
	{
		if (DewPlayer.local.selectedAccessories.Contains(accName))
		{
			return;
		}
		List<string> accs = DewPlayer.local.selectedAccessories.ToList();
		AccType accType = DewResources.GetByName<Accessory>(accName).type;
		for (int i = accs.Count - 1; i >= 0; i--)
		{
			if (DewResources.GetByName<Accessory>(accs[i]).type == accType)
			{
				accs.RemoveAt(i);
			}
		}
		accs.Add(accName);
		DewSave.profile.heroEquippedAccs[DewPlayer.local.selectedHeroType] = accs.ToList();
		DewPlayer.local.CmdSetAccessories(accs);
		DewEffect.Play(fxNametagChange);
	}

	private void RemoveAcc(string accName)
	{
		if (DewPlayer.local.selectedAccessories.Contains(accName))
		{
			List<string> accs = DewPlayer.local.selectedAccessories.ToList();
			accs.Remove(accName);
			DewSave.profile.heroEquippedAccs[DewPlayer.local.selectedHeroType] = accs.ToList();
			DewPlayer.local.CmdSetAccessories(accs);
			DewEffect.Play(fxNametagChange);
		}
	}

	private void UpdateSelected()
	{
		if (DewPlayer.local == null)
		{
			return;
		}
		foreach (UI_AccList_Item item in shownItems)
		{
			if (!(item == null))
			{
				UI_Toggle toggle = item.GetComponent<UI_Toggle>();
				if (!(toggle == null))
				{
					toggle.isChecked = DewPlayer.local.selectedAccessories.Contains(item.accName);
				}
			}
		}
	}
}
