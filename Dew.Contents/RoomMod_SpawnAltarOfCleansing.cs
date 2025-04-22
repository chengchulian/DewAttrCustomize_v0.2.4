public class RoomMod_SpawnAltarOfCleansing : RoomModifierBase
{
	public override void OnStartServer()
	{
		base.OnStartServer();
		PlaceShrine<Shrine_AltarOfCleansing>(new PlaceShrineSettings
		{
			removeModifierOnUse = false
		});
	}

	public override bool IsAvailableInGame()
	{
		return NetworkedManagerBase<GameManager>.instance.isCleanseEnabled;
	}

	private void MirrorProcessed()
	{
	}
}
