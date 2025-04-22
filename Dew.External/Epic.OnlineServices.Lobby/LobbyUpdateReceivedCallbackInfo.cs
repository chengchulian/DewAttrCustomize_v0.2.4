namespace Epic.OnlineServices.Lobby;

public struct LobbyUpdateReceivedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String LobbyId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref LobbyUpdateReceivedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LobbyId = other.LobbyId;
	}
}
