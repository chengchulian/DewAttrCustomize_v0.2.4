namespace Epic.OnlineServices.Lobby;

public struct LobbyInviteRejectedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String InviteId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public Utf8String LobbyId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref LobbyInviteRejectedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		InviteId = other.InviteId;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		LobbyId = other.LobbyId;
	}
}
