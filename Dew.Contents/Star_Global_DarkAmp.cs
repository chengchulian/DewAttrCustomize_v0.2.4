public class Star_Global_DarkAmp : DewStarItemOld
{
	public static float[] BonusAmpPerStack = new float[4] { 0.01f, 0.02f, 0.03f, 0.04f };

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
			darkEffectAmpFlat = BonusAmpPerStack.GetClamped(base.level - 1)
		});
	}
}
