namespace Epic.OnlineServices.Lobby;

public struct LeaveLobbyOptions
{
	public ProductUserId LocalUserId { get; set; }

	public Utf8String LobbyId { get; set; }
}
