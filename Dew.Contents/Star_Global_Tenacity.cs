public class Star_Global_Tenacity : DewStarItemOld
{
	public static readonly float[] BonusTenacity = new float[3] { 5f, 10f, 15f };

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
			tenacityFlat = BonusTenacity.GetClamped(base.level - 1)
		});
	}
}
