public class Star_Global_Cleanse : DewStarItemOld
{
	public static readonly float[] DreamDustRefundMultiplier = new float[3] { 0.7f, 0.8f, 0.9f };

	public override int maxLevel => 3;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		NetworkedManagerBase<GameManager>.instance.isCleanseEnabled = true;
		base.player.cleanseRefundMultiplier = DreamDustRefundMultiplier.GetClamped(base.level - 1);
	}
}
