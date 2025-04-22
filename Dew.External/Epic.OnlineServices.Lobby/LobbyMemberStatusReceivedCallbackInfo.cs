namespace Epic.OnlineServices.Lobby;

public struct LobbyMemberStatusReceivedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String LobbyId { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public LobbyMemberStatus CurrentStatus { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref LobbyMemberStatusReceivedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LobbyId = other.LobbyId;
		TargetUserId = other.TargetUserId;
		CurrentStatus = other.CurrentStatus;
	}
}
