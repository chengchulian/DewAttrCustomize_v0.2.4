using System;
using UnityEngine;

public class PropEnt_Merchant_Jonas : PropEnt_Merchant_Base
{
	public int skillTypeCount = 3;

	public int gemTypeCount = 3;

	public Vector2 gemQuantity;

	public Vector2 skillQuantity;

	private int _fleeDamageCount;

	protected override void OnCreate()
	{
		base.OnCreate();
		if (!base.isServer)
		{
			return;
		}
		EntityEvent_OnTakeDamage += (Action<EventInfoDamage>)delegate(EventInfoDamage obj)
		{
			if (!(obj.actor is ElementalStatusEffect))
			{
				_fleeDamageCount++;
			}
		};
		Dew.CallDelayed(delegate
		{
			if (_merchandises.Count > 0)
			{
				return;
			}
			foreach (DewPlayer current in DewPlayer.humanPlayers)
			{
				PopulatePlayerMerchandises(current);
				remainingRefreshes[current.netId] = current.allowedShopRefreshes;
			}
		});
	}

	protected override void AIUpdate(ref EntityAIContext context)
	{
		base.AIUpdate(ref context);
		bool fleeCondition = _fleeDamageCount >= 60 || base.Status.missingHealth / base.Status.maxHealth >= 0.3f;
		if (base.AI.Helper_CanBeCast<At_Prop_Merchant_Flee>() && fleeCondition)
		{
			base.AI.Helper_CastAbilityAuto<At_Prop_Merchant_Flee>();
		}
	}

	private MerchandiseData GetSkill()
	{
		Loot_Skill lootInstance = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Skill>();
		Rarity rarity = lootInstance.SelectRarityNormal();
		lootInstance.SelectSkillAndLevel(rarity, out var skill, out var skillLevel);
		MerchandiseData result = default(MerchandiseData);
		result.type = MerchandiseType.Skill;
		result.itemName = skill.GetType().Name;
		result.level = skillLevel;
		result.count = Mathf.Max(1, Mathf.RoundToInt(global::UnityEngine.Random.Range(skillQuantity.x, skillQuantity.y)));
		return result;
	}

	private MerchandiseData GetGem()
	{
		Loot_Gem lootInstance = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Gem>();
		Rarity rarity = lootInstance.SelectRarityNormal();
		lootInstance.SelectGemAndQuality(rarity, out var gem, out var gemQuality);
		MerchandiseData result = default(MerchandiseData);
		result.type = MerchandiseType.Gem;
		result.itemName = gem.GetType().Name;
		result.level = gemQuality;
		result.count = Mathf.Max(1, Mathf.RoundToInt(global::UnityEngine.Random.Range(gemQuantity.x, gemQuantity.y)));
		return result;
	}

	private void UpdateItemPrices(DewPlayer player, MerchandiseData[] arr)
	{
		for (int i = 0; i < arr.Length; i++)
		{
			MerchandiseData temp = arr[i];
			if (temp.type == MerchandiseType.Gem)
			{
				Gem gem = DewResources.GetByShortTypeName<Gem>(temp.itemName);
				temp.price = Cost.Gold(Gem.GetBuyGold(player, gem.rarity, temp.level));
			}
			else if (temp.type == MerchandiseType.Skill)
			{
				SkillTrigger skill = DewResources.GetByShortTypeName<SkillTrigger>(temp.itemName);
				temp.price = Cost.Gold(SkillTrigger.GetBuyGold(player, skill.rarity, temp.level));
			}
			else
			{
				temp.price = Cost.Gold(99999);
			}
			arr[i] = temp;
		}
	}

	protected override void OnRefresh(DewPlayer player)
	{
		base.OnRefresh(player);
		PopulatePlayerMerchandises(player);
	}

	private MerchandiseData[] GetBaseSkills()
	{
		MerchandiseData[] baseSkills = new MerchandiseData[skillTypeCount];
		for (int i = 0; i < skillTypeCount; i++)
		{
			baseSkills[i] = GetSkill();
		}
		return baseSkills;
	}

	private MerchandiseData[] GetBaseGems()
	{
		MerchandiseData[] baseGems = new MerchandiseData[gemTypeCount];
		for (int i = 0; i < gemTypeCount; i++)
		{
			baseGems[i] = GetGem();
		}
		return baseGems;
	}

	private void PopulatePlayerMerchandises(DewPlayer player)
	{
		MerchandiseData[] baseSkills = GetBaseSkills();
		MerchandiseData[] baseGems = GetBaseGems();
		MerchandiseData[] arr = new MerchandiseData[skillTypeCount + gemTypeCount + player.shopAddedItems * 2];
		Array.Copy(baseSkills, 0, arr, 0, baseSkills.Length);
		int num = skillTypeCount;
		int to = num + player.shopAddedItems;
		for (int i = num; i < to; i++)
		{
			arr[i] = GetSkill();
		}
		Array.Copy(baseGems, 0, arr, skillTypeCount + player.shopAddedItems, baseGems.Length);
		int num2 = skillTypeCount + player.shopAddedItems + gemTypeCount;
		to = num2 + player.shopAddedItems;
		for (int j = num2; j < to; j++)
		{
			arr[j] = GetGem();
		}
		UpdateItemPrices(player, arr);
		_merchandises[player.netId] = arr;
	}

	private void MirrorProcessed()
	{
	}
}
