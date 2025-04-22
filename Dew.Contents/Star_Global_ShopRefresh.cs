public class Star_Global_ShopRefresh : DewStarItemOld
{
	public override int maxLevel => 1;

	public override bool ShouldInitInGame()
	{
		return base.isServer;
	}

	public override void OnStartInGame()
	{
		base.OnStartInGame();
		base.player.allowedShopRefreshes = 1;
	}
}
