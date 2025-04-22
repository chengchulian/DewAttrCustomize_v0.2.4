public class RoomMod_SpawnGuidance : RoomModifierBase
{
	private Shrine_Guidance _shrine;

	public override void OnStartServer()
	{
		base.OnStartServer();
		PlaceShrine<Shrine_Guidance>(new PlaceShrineSettings
		{
			removeModifierOnUse = true
		});
	}

	private void MirrorProcessed()
	{
	}
}
