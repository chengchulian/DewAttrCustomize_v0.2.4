public class Se_Treasure_BlueElixir_ApBoost : StackedStatusEffect
{
	public int grantedAp;

	public int riftTravelCount;

	private int _currentTravelCount;

	protected override void OnCreate()
	{
	}

	protected override void OnDestroyActor()
	{
	}

	private void ClientEventOnRoomLoaded(EventInfoLoadRoom obj)
	{
	}

	private void MirrorProcessed()
	{
	}
}
