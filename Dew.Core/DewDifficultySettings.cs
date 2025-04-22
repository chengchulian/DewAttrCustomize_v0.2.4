using UnityEngine;

[CreateAssetMenu(fileName = "New Dew Difficulty Settings", menuName = "Dew Difficulty Settings")]
[DewResourceLink(ResourceLinkBy.Name)]
public class DewDifficultySettings : ScriptableObject
{
	public Color difficultyColor;

	public Sprite icon;

	public float iconScale = 1f;

	public float maxPopulationMultiplier;

	public float regenOrbChanceMultiplier;

	public AnimationCurve predictionStrengthCurve;

	public float healRawMultiplier;

	public float scoreMultiplier;

	public float specialSkillChanceMultiplier = 1f;

	public int gainedStardustAmountOffset;

	public bool enableBleedOuts = true;

	public Vector2Int lostSoulDistance;

	public float beneficialNodeMultiplier = 1f;

	public float harmfulNodeMultiplier = 1f;

	public float hunterSpreadChance;

	public float enemyHealthPercentage;

	public float enemyPowerPercentage;

	public float enemyMovementSpeedPercentage;

	public float enemyAttackSpeedPercentage;

	public float enemyAbilityHasteFlat;

	public float scalingFactor = 1f;

	public int positionSampleCount;

	public int positionSampleLagBehindFrames;

	public float positionSampleInterval;

	internal void ApplyDifficultyModifiers(Entity entity)
	{
		if (entity is Monster)
		{
			entity.Status.AddStatBonus(new StatBonus
			{
				maxHealthPercentage = enemyHealthPercentage,
				attackDamagePercentage = enemyPowerPercentage,
				abilityPowerPercentage = enemyPowerPercentage,
				movementSpeedPercentage = enemyMovementSpeedPercentage,
				attackSpeedPercentage = enemyAttackSpeedPercentage,
				abilityHasteFlat = enemyAbilityHasteFlat
			});
		}
		if (entity is Monster m)
		{
			float healthMultiplier;
			float damageMultiplier;
			if (m is BossMonster)
			{
				healthMultiplier = NetworkedManagerBase<GameManager>.instance.GetBossMonsterHealthMultiplierByScaling(scalingFactor);
				damageMultiplier = NetworkedManagerBase<GameManager>.instance.GetBossMonsterDamageMultiplierByScaling(scalingFactor);
			}
			else if (m.type == Monster.MonsterType.MiniBoss)
			{
				healthMultiplier = NetworkedManagerBase<GameManager>.instance.GetMiniBossMonsterHealthMultiplierByScaling(scalingFactor);
				damageMultiplier = NetworkedManagerBase<GameManager>.instance.GetMiniBossMonsterDamageMultiplierByScaling(scalingFactor);
			}
			else
			{
				healthMultiplier = NetworkedManagerBase<GameManager>.instance.GetRegularMonsterHealthMultiplierByScaling(scalingFactor);
				damageMultiplier = NetworkedManagerBase<GameManager>.instance.GetRegularMonsterDamageMultiplierByScaling(scalingFactor);
			}
			entity.Status.AddStatBonus(new StatBonus
			{
				maxHealthPercentage = (healthMultiplier - 1f) * 100f,
				attackDamagePercentage = (damageMultiplier - 1f) * 100f,
				abilityPowerPercentage = (damageMultiplier - 1f) * 100f
			});
		}
		else if (entity is Hero)
		{
			entity.takenHealProcessor.Add(delegate(ref HealData data, Actor actor, Entity target)
			{
				data.ApplyRawMultiplier(NetworkedManagerBase<GameManager>.instance.difficulty.healRawMultiplier);
			}, 100);
		}
	}
}
