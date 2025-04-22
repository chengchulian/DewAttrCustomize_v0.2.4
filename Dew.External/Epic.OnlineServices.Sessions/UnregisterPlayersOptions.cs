namespace Epic.OnlineServices.Sessions;

public struct UnregisterPlayersOptions
{
	public Utf8String SessionName { get; set; }

	public ProductUserId[] PlayersToUnregister { get; set; }
}
