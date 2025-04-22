namespace Epic.OnlineServices.P2P;

public struct SocketId
{
	public string SocketName { get; set; }

	internal void Set(ref SocketIdInternal other)
	{
		SocketName = other.SocketName;
	}
}
