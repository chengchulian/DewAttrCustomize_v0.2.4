using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Lobby_DejavuWindow : MonoBehaviour, IGamepadNavigationHint
{
	public UI_Lobby_DejavuWindow_Item itemPrefab;

	public Transform itemsParentTemplate;

	public GameObject detailEmptyObject;

	public GameObject detailNonEmptyObject;

	public UI_Achievement_Icon detailItemIcon;

	public TextMeshProUGUI detailItemNameText;

	public GameObject[] detailOnStars;

	public GameObject[] detailOffStars;

	public GameObject[] detailPerfectStars;

	public TextMeshProUGUI detailCostText;

	public TextMeshProUGUI detailDescriptionText;

	public GameObject detailInsufficientStardustObject;

	private List<global::UnityEngine.Object> _targets = new List<global::UnityEngine.Object>();

	private List<UI_Lobby_DejavuWindow_Item> _items = new List<UI_Lobby_DejavuWindow_Item>();

	private void Start()
	{
		detailEmptyObject.SetActive(value: true);
		detailNonEmptyObject.SetActive(value: false);
		if (DewBuildProfile.current.buildType == BuildType.DemoLite)
		{
			return;
		}
		itemsParentTemplate.gameObject.SetActive(value: false);
		Transform skillParent = global::UnityEngine.Object.Instantiate(itemsParentTemplate, itemsParentTemplate.parent);
		Transform gemParent = global::UnityEngine.Object.Instantiate(itemsParentTemplate, itemsParentTemplate.parent);
		skillParent.gameObject.SetActive(value: true);
		gemParent.gameObject.SetActive(value: true);
		List<SkillTrigger> skills = new List<SkillTrigger>();
		foreach (KeyValuePair<string, DewProfile.UnlockData> s in DewSave.profile.skills)
		{
			if (!Dew.IsSkillIncludedInGame(s.Key))
			{
				continue;
			}
			SkillTrigger target = DewResources.GetByShortTypeName<SkillTrigger>(s.Key);
			if (!(target == null))
			{
				Rarity rarity = target.rarity;
				if (rarity != Rarity.Character && rarity != Rarity.Identity && s.Value.status == UnlockStatus.Complete)
				{
					skills.Add(target);
				}
			}
		}
		skills.Sort((SkillTrigger x, SkillTrigger y) => x.rarity.CompareTo(y.rarity));
		foreach (SkillTrigger target2 in skills)
		{
			_targets.Add(target2);
			UI_Lobby_DejavuWindow_Item newItem = global::UnityEngine.Object.Instantiate(itemPrefab, skillParent);
			_items.Add(newItem);
			newItem.Setup(target2, _targets.Count - 1);
		}
		List<Gem> gems = new List<Gem>();
		foreach (KeyValuePair<string, DewProfile.UnlockData> g in DewSave.profile.gems)
		{
			if (Dew.IsGemIncludedInGame(g.Key))
			{
				Gem target3 = DewResources.GetByShortTypeName<Gem>(g.Key);
				if (!(target3 == null) && g.Value.status == UnlockStatus.Complete)
				{
					gems.Add(target3);
				}
			}
		}
		gems.Sort((Gem x, Gem y) => x.rarity.CompareTo(y.rarity));
		foreach (Gem target4 in gems)
		{
			_targets.Add(target4);
			UI_Lobby_DejavuWindow_Item newItem2 = global::UnityEngine.Object.Instantiate(itemPrefab, gemParent);
			_items.Add(newItem2);
			newItem2.Setup(target4, _targets.Count - 1);
		}
		Dew.CallOnReady(this, () => DewPlayer.local != null, delegate
		{
			DewPlayer.local.ClientEvent_OnSelectedDejavuItemChanged += new Action<string>(ClientEventOnSelectedDejavuItemChanged);
			if (!string.IsNullOrEmpty(DewSave.profile.preferredDejavuItem) && Dew.IsDejavuFree(DewResources.GetByShortTypeName(DewSave.profile.preferredDejavuItem)))
			{
				DewPlayer.local.CmdSetDejavuItem(DewSave.profile.preferredDejavuItem);
				return;
			}
			foreach (KeyValuePair<string, DewProfile.ItemStatistics> current in DewSave.profile.itemStatistics)
			{
				if (Dew.IsDejavuFree(current.Value))
				{
					DewPlayer.local.CmdSetDejavuItem(current.Key);
					break;
				}
			}
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
		if (obj == null)
		{
			detailEmptyObject.SetActive(value: true);
			detailNonEmptyObject.SetActive(value: false);
			return;
		}
		detailEmptyObject.SetActive(value: false);
		detailNonEmptyObject.SetActive(value: true);
		global::UnityEngine.Object target = DewResources.GetByShortTypeName(obj);
		detailItemIcon.SetupByItem(target);
		if (target is SkillTrigger st)
		{
			detailItemNameText.text = DewLocalization.GetSkillName(st, 0);
			detailItemNameText.color = Dew.GetRarityColor(st.rarity);
		}
		else if (target is Gem g)
		{
			detailItemNameText.text = DewLocalization.GetGemName(DewLocalization.GetGemKey(target.GetType())) + " 100%";
			detailItemNameText.color = Dew.GetRarityColor(g.rarity);
		}
		int originalCost = Dew.GetDejavuCost(target, 1);
		int currentCost = ((!Dew.IsDejavuFree(target)) ? Dew.GetDejavuCost(target) : 0);
		if (originalCost == currentCost)
		{
			detailCostText.text = $"{originalCost:#,##0}";
		}
		else
		{
			detailCostText.text = $"<size=95%><alpha=777><s>{originalCost:#,##0}</s><alpha=fff><size=100%> {currentCost:#,##0}";
		}
		DewProfile.ItemStatistics data = DewSave.profile.itemStatistics[target.GetType().Name];
		int maxWins = Dew.GetDejavuMaxWins(target);
		if (data.wins >= maxWins)
		{
			detailOnStars.SetActiveAll(value: false);
			for (int i = 0; i < detailPerfectStars.Length; i++)
			{
				detailPerfectStars[i].SetActive(maxWins > i);
			}
		}
		else
		{
			detailPerfectStars.SetActiveAll(value: false);
			for (int j = 0; j < detailOnStars.Length; j++)
			{
				detailOnStars[j].SetActive(data.wins > j);
			}
		}
		for (int k = 0; k < detailOffStars.Length; k++)
		{
			detailOffStars[k].SetActive(k < maxWins - data.wins);
		}
		string generalDesc = DewLocalization.GetUIValue((target is SkillTrigger) ? "Dejavu_Description_Memory" : "Dejavu_Description_Essence");
		string priceDesc = (Dew.IsDejavuFree(target) ? ("<color=#f4ff94>" + DewLocalization.GetUIValue((target is SkillTrigger) ? "Dejavu_Description_Discounted_Memory" : "Dejavu_Description_Discounted_Essence") + "</color>") : DewLocalization.GetUIValue((target is SkillTrigger) ? "Dejavu_Description_DiscountOnPurchase_Memory" : "Dejavu_Description_DiscountOnPurchase_Essence"));
		TimeSpan remainingTime = data.dejavuCostReductionPeriodTimestamp.ToDateTime() - DateTime.UtcNow;
		string discountDesc;
		if (data.wins < maxWins)
		{
			int nextCost = Dew.GetDejavuCost(target, data.wins + 1);
			discountDesc = string.Format(DewLocalization.GetUIValue((target is SkillTrigger) ? "Dejavu_Discount_Memory" : "Dejavu_Discount_Essence"), Dew.GetDejavuCost(target) - nextCost, originalCost - Dew.GetDejavuCost(target));
		}
		else
		{
			discountDesc = string.Format(DewLocalization.GetUIValue((target is SkillTrigger) ? "Dejavu_DiscountMaxed_Memory" : "Dejavu_DiscountMaxed_Essence"), originalCost - Dew.GetDejavuCost(target));
		}
		detailDescriptionText.text = generalDesc + "\n" + string.Format(priceDesc, Dew.GetReadableTimespan(remainingTime)) + "\n" + discountDesc;
		detailInsufficientStardustObject.SetActive(DewSave.profile.stardust < currentCost);
	}

	public bool TryGetUp(out IGamepadFocusable next)
	{
		return Get(Vector2.up, out next);
	}

	public bool TryGetLeft(out IGamepadFocusable next)
	{
		return Get(Vector2.left, out next);
	}

	public bool TryGetRight(out IGamepadFocusable next)
	{
		return Get(Vector2.right, out next);
	}

	public bool TryGetDown(out IGamepadFocusable next)
	{
		return Get(Vector2.down, out next);
	}

	private bool Get(Vector2 direction, out IGamepadFocusable next)
	{
		next = null;
		if (ManagerBase<GlobalUIManager>.instance.focused == null)
		{
			return false;
		}
		Vector3 prevPos = ManagerBase<GlobalUIManager>.instance.focused.GetTransform().position;
		float bestScore = float.NegativeInfinity;
		foreach (UI_Lobby_DejavuWindow_Item item in _items)
		{
			Vector3 pos = item.transform.position;
			if (!(Vector2.Dot((pos - prevPos).normalized, direction) < 0.99f))
			{
				float score = 0f - Vector2.Distance(pos, prevPos);
				if (score > bestScore)
				{
					bestScore = score;
					next = item.GetComponent<IGamepadFocusable>();
				}
			}
		}
		return next != null;
	}
}
