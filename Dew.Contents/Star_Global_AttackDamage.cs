public class Star_Global_AttackDamage : DewStarItemOld
{
	public static readonly int[] AttackDamageBonus = new int[5] { 2, 4, 6, 8, 10 };

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
			attackDamageFlat = AttackDamageBonus.GetClamped(base.level - 1)
		});
	}
}
