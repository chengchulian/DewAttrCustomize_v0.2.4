public class Star_Global_SkillHaste : DewStarItemOld
{
	public static int[] BonusSkillHaste = new int[3] { 5, 10, 15 };

	public override int maxLevel => 3;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.hero.Status.AddStatBonus(new StatBonus
		{
			abilityHasteFlat = BonusSkillHaste.GetClamped(base.level - 1)
		});
	}
}
