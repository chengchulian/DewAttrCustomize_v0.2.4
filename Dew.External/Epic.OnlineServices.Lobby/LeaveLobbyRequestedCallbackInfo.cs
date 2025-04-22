namespace Epic.OnlineServices.Lobby;

public struct LeaveLobbyRequestedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String LobbyId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref LeaveLobbyRequestedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		LobbyId = other.LobbyId;
	}
}
