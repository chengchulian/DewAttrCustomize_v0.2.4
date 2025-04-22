public class Star_Global_StartGold : DewStarItemOld
{
	public static readonly int[] GoldAmount = new int[3] { 30, 60, 100 };

	public override int maxLevel => 3;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.player.EarnGold(GoldAmount.GetClamped(base.level - 1));
	}
}
