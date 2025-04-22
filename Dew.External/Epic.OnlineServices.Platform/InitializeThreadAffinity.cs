namespace Epic.OnlineServices.Platform;

public struct InitializeThreadAffinity
{
	public ulong NetworkWork { get; set; }

	public ulong StorageIo { get; set; }

	public ulong WebSocketIo { get; set; }

	public ulong P2PIo { get; set; }

	public ulong HttpRequestIo { get; set; }

	public ulong RTCIo { get; set; }

	internal void Set(ref InitializeThreadAffinityInternal other)
	{
		NetworkWork = other.NetworkWork;
		StorageIo = other.StorageIo;
		WebSocketIo = other.WebSocketIo;
		P2PIo = other.P2PIo;
		HttpRequestIo = other.HttpRequestIo;
		RTCIo = other.RTCIo;
	}
}
