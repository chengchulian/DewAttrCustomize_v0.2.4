namespace Epic.OnlineServices.Lobby;

public struct GetRTCRoomNameOptions
{
	public Utf8String LobbyId { get; set; }

	public ProductUserId LocalUserId { get; set; }
}
