public class Rev_ReachNightSky : DewReverieItem
{
	public override int grantedStardust => 25;

	public override void OnStartLocalClient()
	{
		base.OnStartLocalClient();
		AchCompleteWhen(() => NetworkedManagerBase<ZoneManager>.instance != null && NetworkedManagerBase<ZoneManager>.instance.currentZone != null && NetworkedManagerBase<ZoneManager>.instance.currentZone.name == "Zone_Sky");
	}
}
