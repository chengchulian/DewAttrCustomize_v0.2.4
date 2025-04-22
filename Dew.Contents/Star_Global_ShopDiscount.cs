public class Star_Global_ShopDiscount : DewStarItemOld
{
	public static readonly float[] BonusRatio = new float[4] { 0.05f, 0.07f, 0.1f, 0.15f };

	public override int maxLevel => 4;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.player.buyPriceMultiplier = 1f - BonusRatio.GetClamped(base.level - 1);
	}
}
