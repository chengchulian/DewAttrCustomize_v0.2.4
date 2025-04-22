using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Loot_Skill : Loot
{
	public PerRarityData<float> skillRarityChance;

	public PerRarityData<float> skillRarityChanceHigh;

	public PerRarityData<Formula> skillLevelMinByZoneIndex;

	public PerRarityData<Formula> skillLevelMaxByZoneIndex;

	public AnimationCurve levelRandomCurve;

	public override IEnumerator OnLootRoutine(ClearSectionEventData data)
	{
		Rarity rarity = SelectRarityNormal();
		foreach (DewPlayer humanPlayer in DewPlayer.humanPlayers)
		{
			_ = humanPlayer;
			Vector3 spawnPos = Loot.GetSpawnPos(data);
			SelectSkillAndLevel(rarity, out var skill, out var finalLevel);
			Dew.CreateSkillTrigger(skill, spawnPos, finalLevel);
			yield return new WaitForSeconds(0.2f);
		}
	}

	public Rarity SelectRarityNormal()
	{
		return Loot.SelectRarity(skillRarityChance);
	}

	public Rarity SelectRarityHigh()
	{
		return Loot.SelectRarity(skillRarityChanceHigh);
	}

	public void SelectSkillAndLevel(Rarity rarity, out SkillTrigger skill, out int level)
	{
		HashSet<string> pool = NetworkedManagerBase<LootManager>.instance.poolSkillsByRarity[rarity];
		skill = DewResources.GetByShortTypeName<SkillTrigger>(pool.ElementAt(Random.Range(0, pool.Count)));
		float a = skillLevelMinByZoneIndex.Get(rarity).Evaluate(NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex);
		float max = skillLevelMaxByZoneIndex.Get(rarity).Evaluate(NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex);
		float floatLevel = Mathf.Lerp(a, max, levelRandomCurve.Evaluate(Random.value));
		level = Mathf.Clamp(Mathf.RoundToInt(floatLevel), 1, 100);
	}
}
