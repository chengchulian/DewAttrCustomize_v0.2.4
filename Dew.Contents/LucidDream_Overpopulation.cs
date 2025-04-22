public class LucidDream_Overpopulation : LucidDream
{
    protected override void OnCreate()
    {
        base.OnCreate();
        if (base.isServer)
        {
            NetworkedManagerBase<GameManager>.instance.maxAndSpawnedPopulationMultiplier = AttrCustomizeResources.Config.maxAndSpawnedPopulationMultiplier;
        }
    }

    protected override void OnDestroyActor()
    {
        base.OnDestroyActor();
        if (base.isServer)
        {
            NetworkedManagerBase<GameManager>.instance.maxAndSpawnedPopulationMultiplier = 1f;
        }
    }

    private void MirrorProcessed()
    {
    }
}