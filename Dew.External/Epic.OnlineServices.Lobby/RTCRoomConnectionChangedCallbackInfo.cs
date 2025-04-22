namespace Epic.OnlineServices.Lobby;

public struct RTCRoomConnectionChangedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String LobbyId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public bool IsConnected { get; set; }

	public Result DisconnectReason { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref RTCRoomConnectionChangedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LobbyId = other.LobbyId;
		LocalUserId = other.LocalUserId;
		IsConnected = other.IsConnected;
		DisconnectReason = other.DisconnectReason;
	}
}
