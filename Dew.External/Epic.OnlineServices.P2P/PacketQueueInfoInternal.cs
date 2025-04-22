using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PacketQueueInfoInternal : IGettable<PacketQueueInfo>, ISettable<PacketQueueInfo>, IDisposable
{
	private ulong m_IncomingPacketQueueMaxSizeBytes;

	private ulong m_IncomingPacketQueueCurrentSizeBytes;

	private ulong m_IncomingPacketQueueCurrentPacketCount;

	private ulong m_OutgoingPacketQueueMaxSizeBytes;

	private ulong m_OutgoingPacketQueueCurrentSizeBytes;

	private ulong m_OutgoingPacketQueueCurrentPacketCount;

	public ulong IncomingPacketQueueMaxSizeBytes
	{
		get
		{
			return m_IncomingPacketQueueMaxSizeBytes;
		}
		set
		{
			m_IncomingPacketQueueMaxSizeBytes = value;
		}
	}

	public ulong IncomingPacketQueueCurrentSizeBytes
	{
		get
		{
			return m_IncomingPacketQueueCurrentSizeBytes;
		}
		set
		{
			m_IncomingPacketQueueCurrentSizeBytes = value;
		}
	}

	public ulong IncomingPacketQueueCurrentPacketCount
	{
		get
		{
			return m_IncomingPacketQueueCurrentPacketCount;
		}
		set
		{
			m_IncomingPacketQueueCurrentPacketCount = value;
		}
	}

	public ulong OutgoingPacketQueueMaxSizeBytes
	{
		get
		{
			return m_OutgoingPacketQueueMaxSizeBytes;
		}
		set
		{
			m_OutgoingPacketQueueMaxSizeBytes = value;
		}
	}

	public ulong OutgoingPacketQueueCurrentSizeBytes
	{
		get
		{
			return m_OutgoingPacketQueueCurrentSizeBytes;
		}
		set
		{
			m_OutgoingPacketQueueCurrentSizeBytes = value;
		}
	}

	public ulong OutgoingPacketQueueCurrentPacketCount
	{
		get
		{
			return m_OutgoingPacketQueueCurrentPacketCount;
		}
		set
		{
			m_OutgoingPacketQueueCurrentPacketCount = value;
		}
	}

	public void Set(ref PacketQueueInfo other)
	{
		IncomingPacketQueueMaxSizeBytes = other.IncomingPacketQueueMaxSizeBytes;
		IncomingPacketQueueCurrentSizeBytes = other.IncomingPacketQueueCurrentSizeBytes;
		IncomingPacketQueueCurrentPacketCount = other.IncomingPacketQueueCurrentPacketCount;
		OutgoingPacketQueueMaxSizeBytes = other.OutgoingPacketQueueMaxSizeBytes;
		OutgoingPacketQueueCurrentSizeBytes = other.OutgoingPacketQueueCurrentSizeBytes;
		OutgoingPacketQueueCurrentPacketCount = other.OutgoingPacketQueueCurrentPacketCount;
	}

	public void Set(ref PacketQueueInfo? other)
	{
		if (other.HasValue)
		{
			IncomingPacketQueueMaxSizeBytes = other.Value.IncomingPacketQueueMaxSizeBytes;
			IncomingPacketQueueCurrentSizeBytes = other.Value.IncomingPacketQueueCurrentSizeBytes;
			IncomingPacketQueueCurrentPacketCount = other.Value.IncomingPacketQueueCurrentPacketCount;
			OutgoingPacketQueueMaxSizeBytes = other.Value.OutgoingPacketQueueMaxSizeBytes;
			OutgoingPacketQueueCurrentSizeBytes = other.Value.OutgoingPacketQueueCurrentSizeBytes;
			OutgoingPacketQueueCurrentPacketCount = other.Value.OutgoingPacketQueueCurrentPacketCount;
		}
	}

	public void Dispose()
	{
	}

	public void Get(out PacketQueueInfo output)
	{
		output = default(PacketQueueInfo);
		output.Set(ref this);
	}
}
