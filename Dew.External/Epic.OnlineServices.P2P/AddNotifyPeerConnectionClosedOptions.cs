namespace Epic.OnlineServices.P2P;

public struct AddNotifyPeerConnectionClosedOptions
{
	public ProductUserId LocalUserId { get; set; }

	public SocketId? SocketId { get; set; }
}
