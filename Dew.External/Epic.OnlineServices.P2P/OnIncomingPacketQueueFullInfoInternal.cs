using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnIncomingPacketQueueFullInfoInternal : ICallbackInfoInternal, IGettable<OnIncomingPacketQueueFullInfo>, ISettable<OnIncomingPacketQueueFullInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private ulong m_PacketQueueMaxSizeBytes;

	private ulong m_PacketQueueCurrentSizeBytes;

	private IntPtr m_OverflowPacketLocalUserId;

	private byte m_OverflowPacketChannel;

	private uint m_OverflowPacketSizeBytes;

	public object ClientData
	{
		get
		{
			Helper.Get(m_ClientData, out object value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientData);
		}
	}

	public IntPtr ClientDataAddress => m_ClientData;

	public ulong PacketQueueMaxSizeBytes
	{
		get
		{
			return m_PacketQueueMaxSizeBytes;
		}
		set
		{
			m_PacketQueueMaxSizeBytes = value;
		}
	}

	public ulong PacketQueueCurrentSizeBytes
	{
		get
		{
			return m_PacketQueueCurrentSizeBytes;
		}
		set
		{
			m_PacketQueueCurrentSizeBytes = value;
		}
	}

	public ProductUserId OverflowPacketLocalUserId
	{
		get
		{
			Helper.Get(m_OverflowPacketLocalUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OverflowPacketLocalUserId);
		}
	}

	public byte OverflowPacketChannel
	{
		get
		{
			return m_OverflowPacketChannel;
		}
		set
		{
			m_OverflowPacketChannel = value;
		}
	}

	public uint OverflowPacketSizeBytes
	{
		get
		{
			return m_OverflowPacketSizeBytes;
		}
		set
		{
			m_OverflowPacketSizeBytes = value;
		}
	}

	public void Set(ref OnIncomingPacketQueueFullInfo other)
	{
		ClientData = other.ClientData;
		PacketQueueMaxSizeBytes = other.PacketQueueMaxSizeBytes;
		PacketQueueCurrentSizeBytes = other.PacketQueueCurrentSizeBytes;
		OverflowPacketLocalUserId = other.OverflowPacketLocalUserId;
		OverflowPacketChannel = other.OverflowPacketChannel;
		OverflowPacketSizeBytes = other.OverflowPacketSizeBytes;
	}

	public void Set(ref OnIncomingPacketQueueFullInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			PacketQueueMaxSizeBytes = other.Value.PacketQueueMaxSizeBytes;
			PacketQueueCurrentSizeBytes = other.Value.PacketQueueCurrentSizeBytes;
			OverflowPacketLocalUserId = other.Value.OverflowPacketLocalUserId;
			OverflowPacketChannel = other.Value.OverflowPacketChannel;
			OverflowPacketSizeBytes = other.Value.OverflowPacketSizeBytes;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_OverflowPacketLocalUserId);
	}

	public void Get(out OnIncomingPacketQueueFullInfo output)
	{
		output = default(OnIncomingPacketQueueFullInfo);
		output.Set(ref this);
	}
}
