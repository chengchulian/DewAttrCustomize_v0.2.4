namespace Epic.OnlineServices.P2P;

public struct OnPeerConnectionInterruptedInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId RemoteUserId { get; set; }

	public SocketId? SocketId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnPeerConnectionInterruptedInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RemoteUserId = other.RemoteUserId;
		SocketId = other.SocketId;
	}
}
