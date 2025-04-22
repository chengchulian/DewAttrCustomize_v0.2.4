public class Star_Global_ShopMoreItems : DewStarItemOld
{
    public override int maxLevel => 1;

    public override bool ShouldInitInGame()
    {
        return base.isServer;
    }

    public override void OnStartInGame()
    {
        base.OnStartInGame();
        base.player.shopAddedItems = AttrCustomizeResources.Config.shopAddedItems;
    }
}