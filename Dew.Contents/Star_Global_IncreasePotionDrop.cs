public class Star_Global_IncreasePotionDrop : DewStarItemOld
{
	public static readonly float[] PotionChanceIncrease = new float[3] { 0.35f, 0.7f, 1f };

	public override int maxLevel => 3;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.player.potionDropChanceMultiplier = PotionChanceIncrease.GetClamped(base.level - 1);
	}
}
