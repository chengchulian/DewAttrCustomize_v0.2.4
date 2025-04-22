public class Star_Global_Armor : DewStarItemOld
{
	public static readonly int[] ArmorBonus = new int[5] { 2, 4, 6, 9, 12 };

	public override int maxLevel => 5;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.hero.CreateBasicEffect(base.hero, new ArmorBoostEffect
		{
			strength = ArmorBonus.GetClamped(base.level - 1)
		}, float.PositiveInfinity, "StarArmor");
	}
}
