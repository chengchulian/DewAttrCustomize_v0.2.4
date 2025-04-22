public class Star_Global_AbilityPower : DewStarItemOld
{
	public static readonly int[] AbilityPowerBonus = new int[5] { 3, 6, 9, 12, 15 };

	public override int maxLevel => 5;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.hero.Status.AddStatBonus(new StatBonus
		{
			abilityPowerFlat = AbilityPowerBonus.GetClamped(base.level - 1)
		});
	}
}
