using UnityEngine;

public class Star_Global_StartSkill : DewStarItemOld
{
	public static readonly int[] SkillLevel = new int[3] { 1, 2, 3 };

	public override int maxLevel => 3;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		GameManager.CallOnReady(delegate
		{
			Loot_Skill lootInstance = NetworkedManagerBase<LootManager>.instance.GetLootInstance<Loot_Skill>();
			Rarity rarity = lootInstance.SelectRarityNormal();
			lootInstance.SelectSkillAndLevel(rarity, out var skill, out var _);
			Vector3 pivot = base.hero.agentPosition + (Rift.instance.transform.position - base.hero.agentPosition).normalized * 2.5f;
			pivot = Dew.GetGoodRewardPosition(pivot, 1f);
			Dew.CreateSkillTrigger(skill, pivot, SkillLevel.GetClamped(base.level - 1), base.player);
		});
	}
}
