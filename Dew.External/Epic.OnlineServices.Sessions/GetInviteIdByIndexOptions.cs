namespace Epic.OnlineServices.Sessions;

public struct GetInviteIdByIndexOptions
{
	public ProductUserId LocalUserId { get; set; }

	public uint Index { get; set; }
}
