namespace Epic.OnlineServices.Lobby;

public struct LobbyMemberUpdateReceivedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String LobbyId { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref LobbyMemberUpdateReceivedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LobbyId = other.LobbyId;
		TargetUserId = other.TargetUserId;
	}
}
