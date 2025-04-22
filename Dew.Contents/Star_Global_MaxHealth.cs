public class Star_Global_MaxHealth : DewStarItemOld
{
	public static readonly float[] BonusHealth = new float[4] { 30f, 60f, 100f, 150f };

	public override int maxLevel => 4;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.hero.Status.AddStatBonus(new StatBonus
		{
			maxHealthFlat = BonusHealth.GetClamped(base.level - 1)
		});
	}
}
