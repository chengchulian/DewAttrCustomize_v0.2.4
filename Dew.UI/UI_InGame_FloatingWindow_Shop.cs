using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame_FloatingWindow_Shop : UI_InGame_FloatingWindow_Base
{
	public UI_InGame_FloatingWindow_Shop_Item itemPrefab;

	public Transform itemsParent;

	public GameObject refreshObject;

	public CostDisplay refreshCost;

	public GridLayoutGroup grid;

	private List<UI_InGame_FloatingWindow_Shop_Item> _items = new List<UI_InGame_FloatingWindow_Shop_Item>();

	public override Type GetSupportedType()
	{
		return typeof(PropEnt_Merchant_Base);
	}

	public override void LogicUpdate(float dt)
	{
		base.LogicUpdate(dt);
		UpdateStatus();
	}

	public override void OnActivate()
	{
		base.OnActivate();
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		if (!(ManagerBase<FloatingWindowManager>.softInstance == null))
		{
			UpdateMerchandise();
			UpdateRefresh();
		}
	}

	private void UpdateRefresh()
	{
		MonoBehaviour t = ManagerBase<FloatingWindowManager>.instance.currentTarget;
		if (t == null || !(t is PropEnt_Merchant_Jonas merchant) || merchant.IsNullInactiveDeadOrKnockedOut() || !merchant.CanRefresh(DewPlayer.local))
		{
			refreshObject.SetActive(value: false);
			return;
		}
		int cost = merchant.GetRefreshGoldCost(DewPlayer.local);
		refreshObject.SetActive(value: true);
		refreshCost.SetupGold(cost);
	}

	private void UpdateMerchandise()
	{
		if (ManagerBase<FloatingWindowManager>.instance.currentTarget is PropEnt_Merchant_Base merchant && !merchant.IsNullInactiveDeadOrKnockedOut())
		{
			while (_items.Count > merchant.merchandises[DewPlayer.local.netId].Length)
			{
				global::UnityEngine.Object.Destroy(_items[0].gameObject);
				_items.RemoveAt(0);
			}
			while (_items.Count < merchant.merchandises[DewPlayer.local.netId].Length)
			{
				_items.Add(global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent));
			}
			for (int i = 0; i < merchant.merchandises[DewPlayer.local.netId].Length; i++)
			{
				_items[i].UpdateContent(merchant.merchandises[DewPlayer.local.netId][i], i);
			}
			grid.constraintCount = 3 + DewPlayer.local.shopAddedItems;
		}
	}

	public void ClickMerchandise(int index)
	{
		if (!(base.target is PropEnt_Merchant_Base merchant) || merchant.IsNullOrInactive())
		{
			return;
		}
		MerchandiseData data = merchant.merchandises[DewPlayer.local.netId][index];
		if (data.count > 0)
		{
			AffordType canAfford = ((Cost)(data.price * DewPlayer.local.buyPriceMultiplier)).CanAfford(DewPlayer.local.hero);
			if (canAfford != 0)
			{
				InGameUIManager.instance.ShowCenterMessage(CenterMessageType.Error, "InGame_Message_CannotAfford" + canAfford);
				return;
			}
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Shop_Purchase");
			merchant.CmdPurchase(index);
		}
	}

	public void ClickRefresh()
	{
		if (base.target is PropEnt_Merchant_Jonas merchant && !merchant.IsNullOrInactive() && merchant.CanRefresh(DewPlayer.local))
		{
			int cost = merchant.GetRefreshGoldCost(DewPlayer.local);
			if (DewPlayer.local.gold < cost)
			{
				InGameUIManager.instance.ShowCenterMessage(CenterMessageType.Error, "InGame_Message_NotEnoughGold");
				return;
			}
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Shop_Purchase");
			merchant.CmdRefresh();
		}
	}
}
