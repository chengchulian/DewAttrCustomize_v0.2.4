using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetNextReceivedPacketSizeOptionsInternal : ISettable<GetNextReceivedPacketSizeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RequestedChannel;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public byte? RequestedChannel
	{
		set
		{
			Helper.Set(value, ref m_RequestedChannel);
		}
	}

	public void Set(ref GetNextReceivedPacketSizeOptions other)
	{
		m_ApiVersion = 2;
		LocalUserId = other.LocalUserId;
		RequestedChannel = other.RequestedChannel;
	}

	public void Set(ref GetNextReceivedPacketSizeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LocalUserId = other.Value.LocalUserId;
			RequestedChannel = other.Value.RequestedChannel;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RequestedChannel);
	}
}
