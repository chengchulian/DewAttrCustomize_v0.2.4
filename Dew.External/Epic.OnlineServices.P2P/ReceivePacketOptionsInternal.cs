using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ReceivePacketOptionsInternal : ISettable<ReceivePacketOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private uint m_MaxDataSizeBytes;

	private IntPtr m_RequestedChannel;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public uint MaxDataSizeBytes
	{
		set
		{
			m_MaxDataSizeBytes = value;
		}
	}

	public byte? RequestedChannel
	{
		set
		{
			Helper.Set(value, ref m_RequestedChannel);
		}
	}

	public void Set(ref ReceivePacketOptions other)
	{
		m_ApiVersion = 2;
		LocalUserId = other.LocalUserId;
		MaxDataSizeBytes = other.MaxDataSizeBytes;
		RequestedChannel = other.RequestedChannel;
	}

	public void Set(ref ReceivePacketOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LocalUserId = other.Value.LocalUserId;
			MaxDataSizeBytes = other.Value.MaxDataSizeBytes;
			RequestedChannel = other.Value.RequestedChannel;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RequestedChannel);
	}
}
