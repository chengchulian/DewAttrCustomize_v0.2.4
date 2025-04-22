using System.Collections;
using UnityEngine;

public class Shrine_Disintegration : Shrine
{
	public float goldMultiplier;

	public float addedMultiplierPerUse;

	public override bool isRegularReward => true;

	protected override bool OnUse(Entity entity)
	{
		int currentZoneIndex = NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex;
		Loot_Gem lootInstance = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Gem>();
		float num = lootInstance.gemQualityMinByZoneIndex.Get(Rarity.Rare).Evaluate(currentZoneIndex);
		float num2 = lootInstance.gemQualityMaxByZoneIndex.Get(Rarity.Rare).Evaluate(currentZoneIndex);
		int buyGold = Gem.GetBuyGold(null, Rarity.Rare, Mathf.RoundToInt((num + num2) / 2f));
		int amount = DewMath.RandomRoundToInt((float)buyGold * goldMultiplier * (1f + addedMultiplierPerUse * (float)(base.currentUseCount - 1)));
		StartCoroutine(Routine());
		return true;
		IEnumerator Routine()
		{
			for (int i = 0; i < 5; i++)
			{
				NetworkedManagerBase<PickupManager>.instance.DropGold(isKillGold: false, isGivenByOtherPlayer: false, Mathf.RoundToInt((float)amount / 5f), entity.position, (Hero)entity);
				yield return new WaitForSeconds(0.25f);
			}
		}
	}

	private void MirrorProcessed()
	{
	}
}
