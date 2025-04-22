namespace Epic.OnlineServices.P2P;

public struct SetPacketQueueSizeOptions
{
	public ulong IncomingPacketQueueMaxSizeBytes { get; set; }

	public ulong OutgoingPacketQueueMaxSizeBytes { get; set; }
}
