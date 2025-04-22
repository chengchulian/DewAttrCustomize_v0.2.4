namespace Epic.OnlineServices.P2P;

public struct OnPeerConnectionEstablishedInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId RemoteUserId { get; set; }

	public SocketId? SocketId { get; set; }

	public ConnectionEstablishedType ConnectionType { get; set; }

	public NetworkConnectionType NetworkType { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnPeerConnectionEstablishedInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RemoteUserId = other.RemoteUserId;
		SocketId = other.SocketId;
		ConnectionType = other.ConnectionType;
		NetworkType = other.NetworkType;
	}
}
