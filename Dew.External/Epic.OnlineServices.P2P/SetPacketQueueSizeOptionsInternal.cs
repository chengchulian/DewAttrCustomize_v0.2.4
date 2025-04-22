using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetPacketQueueSizeOptionsInternal : ISettable<SetPacketQueueSizeOptions>, IDisposable
{
	private int m_ApiVersion;

	private ulong m_IncomingPacketQueueMaxSizeBytes;

	private ulong m_OutgoingPacketQueueMaxSizeBytes;

	public ulong IncomingPacketQueueMaxSizeBytes
	{
		set
		{
			m_IncomingPacketQueueMaxSizeBytes = value;
		}
	}

	public ulong OutgoingPacketQueueMaxSizeBytes
	{
		set
		{
			m_OutgoingPacketQueueMaxSizeBytes = value;
		}
	}

	public void Set(ref SetPacketQueueSizeOptions other)
	{
		m_ApiVersion = 1;
		IncomingPacketQueueMaxSizeBytes = other.IncomingPacketQueueMaxSizeBytes;
		OutgoingPacketQueueMaxSizeBytes = other.OutgoingPacketQueueMaxSizeBytes;
	}

	public void Set(ref SetPacketQueueSizeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			IncomingPacketQueueMaxSizeBytes = other.Value.IncomingPacketQueueMaxSizeBytes;
			OutgoingPacketQueueMaxSizeBytes = other.Value.OutgoingPacketQueueMaxSizeBytes;
		}
	}

	public void Dispose()
	{
	}
}
