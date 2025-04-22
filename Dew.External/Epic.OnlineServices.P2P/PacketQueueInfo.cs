namespace Epic.OnlineServices.P2P;

public struct PacketQueueInfo
{
	public ulong IncomingPacketQueueMaxSizeBytes { get; set; }

	public ulong IncomingPacketQueueCurrentSizeBytes { get; set; }

	public ulong IncomingPacketQueueCurrentPacketCount { get; set; }

	public ulong OutgoingPacketQueueMaxSizeBytes { get; set; }

	public ulong OutgoingPacketQueueCurrentSizeBytes { get; set; }

	public ulong OutgoingPacketQueueCurrentPacketCount { get; set; }

	internal void Set(ref PacketQueueInfoInternal other)
	{
		IncomingPacketQueueMaxSizeBytes = other.IncomingPacketQueueMaxSizeBytes;
		IncomingPacketQueueCurrentSizeBytes = other.IncomingPacketQueueCurrentSizeBytes;
		IncomingPacketQueueCurrentPacketCount = other.IncomingPacketQueueCurrentPacketCount;
		OutgoingPacketQueueMaxSizeBytes = other.OutgoingPacketQueueMaxSizeBytes;
		OutgoingPacketQueueCurrentSizeBytes = other.OutgoingPacketQueueCurrentSizeBytes;
		OutgoingPacketQueueCurrentPacketCount = other.OutgoingPacketQueueCurrentPacketCount;
	}
}
