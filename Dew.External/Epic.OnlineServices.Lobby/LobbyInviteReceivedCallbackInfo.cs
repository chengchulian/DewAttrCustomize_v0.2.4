namespace Epic.OnlineServices.Lobby;

public struct LobbyInviteReceivedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String InviteId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref LobbyInviteReceivedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		InviteId = other.InviteId;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
	}
}
