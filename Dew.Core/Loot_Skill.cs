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
		
		if (AttrCustomizeResources.Config.enableHeroSkillAddShop)
		{
			switch (rarity)
			{
				case Rarity.Common:
				{
					List<string> qSkill = new List<string> { "St_Q_GoldenBurst", "St_Q_HandCannon", "St_Q_IncendiaryRounds", "St_Q_Lunge", "St_Q_Fleche", "St_Q_CruelSun", "St_Q_Discipline", "St_Q_Lunge", "St_Q_EtherealInfluence", "St_Q_SuperNova" };
					pool.UnionWith(qSkill);
					break;
				}
				case Rarity.Rare:
				{
					List<string> rSkill = new List<string> { "St_R_DangerousTheory", "St_R_QuickTrigger", "St_R_PrecisionShot", "St_R_UnbreakableDetermination", "St_R_Parry", "St_R_SanctuaryOfEl", "St_R_Cataclysm", "St_R_Tranquility" };
					pool.UnionWith(rSkill);
					break;
				}
				case Rarity.Epic:
				{
					List<string> identitySkill = new List<string> { "St_D_DisintegratingClaw", "St_D_SalamanderPowder", "St_D_DoubleTap", "St_D_AstridsMasterpiece", "St_D_MercyOfEl", "St_D_Resolve", "St_D_ExoticMatter", "St_D_ConvergencePoint" };
					pool.UnionWith(identitySkill);
					break;
				}
			}
		}

		string[] removeSkills = AttrCustomizeResources.Config.removeSkills;
		for (var i = 0; i < removeSkills.Length; i++)
		{
			pool.Remove(removeSkills[i]);
		}
		
		skill = DewResources.GetByShortTypeName<SkillTrigger>(pool.ElementAt(Random.Range(0, pool.Count)));
		float a = skillLevelMinByZoneIndex.Get(rarity).Evaluate(NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex);
		float max = skillLevelMaxByZoneIndex.Get(rarity).Evaluate(NetworkedManagerBase<ZoneManager>.instance.currentZoneIndex);
		float floatLevel = Mathf.Lerp(a, max, levelRandomCurve.Evaluate(Random.value));
		level = Mathf.Clamp(Mathf.RoundToInt(floatLevel), 1, 100);
	}
}
