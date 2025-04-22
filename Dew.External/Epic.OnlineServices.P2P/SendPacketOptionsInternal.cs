using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SendPacketOptionsInternal : ISettable<SendPacketOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RemoteUserId;

	private IntPtr m_SocketId;

	private byte m_Channel;

	private uint m_DataLengthBytes;

	private IntPtr m_Data;

	private int m_AllowDelayedDelivery;

	private PacketReliability m_Reliability;

	private int m_DisableAutoAcceptConnection;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public ProductUserId RemoteUserId
	{
		set
		{
			Helper.Set(value, ref m_RemoteUserId);
		}
	}

	public SocketId? SocketId
	{
		set
		{
			Helper.Set<SocketId, SocketIdInternal>(ref value, ref m_SocketId);
		}
	}

	public byte Channel
	{
		set
		{
			m_Channel = value;
		}
	}

	public ArraySegment<byte> Data
	{
		set
		{
			Helper.Set(value, ref m_Data, out m_DataLengthBytes);
		}
	}

	public bool AllowDelayedDelivery
	{
		set
		{
			Helper.Set(value, ref m_AllowDelayedDelivery);
		}
	}

	public PacketReliability Reliability
	{
		set
		{
			m_Reliability = value;
		}
	}

	public bool DisableAutoAcceptConnection
	{
		set
		{
			Helper.Set(value, ref m_DisableAutoAcceptConnection);
		}
	}

	public void Set(ref SendPacketOptions other)
	{
		m_ApiVersion = 3;
		LocalUserId = other.LocalUserId;
		RemoteUserId = other.RemoteUserId;
		SocketId = other.SocketId;
		Channel = other.Channel;
		Data = other.Data;
		AllowDelayedDelivery = other.AllowDelayedDelivery;
		Reliability = other.Reliability;
		DisableAutoAcceptConnection = other.DisableAutoAcceptConnection;
	}

	public void Set(ref SendPacketOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			LocalUserId = other.Value.LocalUserId;
			RemoteUserId = other.Value.RemoteUserId;
			SocketId = other.Value.SocketId;
			Channel = other.Value.Channel;
			Data = other.Value.Data;
			AllowDelayedDelivery = other.Value.AllowDelayedDelivery;
			Reliability = other.Value.Reliability;
			DisableAutoAcceptConnection = other.Value.DisableAutoAcceptConnection;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RemoteUserId);
		Helper.Dispose(ref m_SocketId);
		Helper.Dispose(ref m_Data);
	}
}
