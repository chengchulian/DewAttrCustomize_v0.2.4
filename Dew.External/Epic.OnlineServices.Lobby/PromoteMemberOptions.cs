namespace Epic.OnlineServices.Lobby;

public struct PromoteMemberOptions
{
	public Utf8String LobbyId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId TargetUserId { get; set; }
}
