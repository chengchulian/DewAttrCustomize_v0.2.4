using UnityEngine;

public static class PerDataHelper
{
	public static PerRarityData<int> GetRandom(PerRarityData<int> min, PerRarityData<int> max)
	{
		PerRarityData<int> result = default(PerRarityData<int>);
		result.common = Random.Range(min.common, max.common + 1);
		result.rare = Random.Range(min.rare, max.rare + 1);
		result.epic = Random.Range(min.epic, max.epic + 1);
		result.legendary = Random.Range(min.legendary, max.legendary + 1);
		return result;
	}

	public static PerRarityData<float> GetRandom(PerRarityData<float> min, PerRarityData<float> max)
	{
		PerRarityData<float> result = default(PerRarityData<float>);
		result.common = Random.Range(min.common, max.common);
		result.rare = Random.Range(min.rare, max.rare);
		result.epic = Random.Range(min.epic, max.epic);
		result.legendary = Random.Range(min.legendary, max.legendary);
		return result;
	}
}
