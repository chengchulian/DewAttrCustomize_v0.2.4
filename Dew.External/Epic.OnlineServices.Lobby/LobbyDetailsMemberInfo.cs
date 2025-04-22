namespace Epic.OnlineServices.Lobby;

public struct LobbyDetailsMemberInfo
{
	public ProductUserId UserId { get; set; }

	public uint Platform { get; set; }

	public bool AllowsCrossplay { get; set; }

	internal void Set(ref LobbyDetailsMemberInfoInternal other)
	{
		UserId = other.UserId;
		Platform = other.Platform;
		AllowsCrossplay = other.AllowsCrossplay;
	}
}
