namespace Epic.OnlineServices.P2P;

public struct OnRemoteConnectionClosedInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId RemoteUserId { get; set; }

	public SocketId? SocketId { get; set; }

	public ConnectionClosedReason Reason { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnRemoteConnectionClosedInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RemoteUserId = other.RemoteUserId;
		SocketId = other.SocketId;
		Reason = other.Reason;
	}
}
