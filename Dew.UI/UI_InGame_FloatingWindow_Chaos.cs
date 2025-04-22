using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_InGame_FloatingWindow_Chaos : UI_InGame_FloatingWindow_Base, IGamepadFocusListener
{
	public UI_InGame_FloatingWindow_Chaos_Item itemPrefab;

	public Transform itemsParent;

	public GameObject gamepadFocusObject;

	private List<UI_InGame_FloatingWindow_Chaos_Item> _items = new List<UI_InGame_FloatingWindow_Chaos_Item>();

	public override Type GetSupportedType()
	{
		return typeof(Shrine_Chaos);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Chaos_Open");
		UpdateItems();
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Chaos_Close");
	}

	private void UpdateItems()
	{
		if (!(base.target == null) && base.target is Shrine_Chaos chaos)
		{
			ChaosReward[] myRewards = chaos.rewards.GetValueOrDefault(DewPlayer.local, Array.Empty<ChaosReward>());
			while (_items.Count > myRewards.Length)
			{
				global::UnityEngine.Object.Destroy(_items[0].gameObject);
				_items.RemoveAt(0);
			}
			while (_items.Count < myRewards.Length)
			{
				_items.Add(global::UnityEngine.Object.Instantiate(itemPrefab, itemsParent));
			}
			for (int i = 0; i < myRewards.Length; i++)
			{
				_items[i].UpdateContent(myRewards[i], i);
			}
		}
	}

	public void ClickChaosItem(int index)
	{
		if (ManagerBase<FloatingWindowManager>.instance.currentTarget is Shrine_Chaos chaos && !chaos.IsNullOrInactive())
		{
			ChaosReward data = chaos.rewards[DewPlayer.local][index];
			ManagerBase<FeedbackManager>.instance.PlayFeedbackEffect("UI_Chaos_Choose");
			if (data.type == ChaosRewardType.UpgradeGem)
			{
				ManagerBase<FloatingWindowManager>.instance.ClearTarget();
				ManagerBase<EditSkillManager>.instance.StartUpgradeGem(chaos, once: true);
			}
			else if (data.type == ChaosRewardType.UpgradeSkill)
			{
				ManagerBase<FloatingWindowManager>.instance.ClearTarget();
				ManagerBase<EditSkillManager>.instance.StartUpgradeSkill(chaos, once: true);
			}
			else
			{
				ManagerBase<FloatingWindowManager>.instance.ClearTarget();
				chaos.CmdChoose(index);
			}
		}
	}

	public void OnFocusStateChanged(bool state)
	{
		if (gamepadFocusObject != null)
		{
			gamepadFocusObject.SetActive(state);
		}
	}
}
