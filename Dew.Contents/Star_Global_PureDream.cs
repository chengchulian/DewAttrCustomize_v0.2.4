public class Star_Global_PureDream : DewStarItemOld
{
	public override int maxLevel => 1;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		NetworkedManagerBase<GameManager>.instance.isPureDreamEnabled = true;
	}
}
