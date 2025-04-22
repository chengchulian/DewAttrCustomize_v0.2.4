public class RoomMod_SpawnHatred : RoomModifierBase
{
	public override void OnStartServer()
	{
		base.OnStartServer();
		SingletonDewNetworkBehaviour<Room>.instance.rewards.DisableRegularRewards();
		PlaceShrine<Shrine_Hatred>(new PlaceShrineSettings
		{
			removeModifierOnUse = true,
			spawnOnLastSection = true,
			lockedUntilCleared = true
		});
	}

	private void MirrorProcessed()
	{
	}
}
