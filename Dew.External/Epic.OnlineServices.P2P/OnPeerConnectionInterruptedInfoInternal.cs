using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnPeerConnectionInterruptedInfoInternal : ICallbackInfoInternal, IGettable<OnPeerConnectionInterruptedInfo>, ISettable<OnPeerConnectionInterruptedInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_RemoteUserId;

	private IntPtr m_SocketId;

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

	public ProductUserId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public ProductUserId RemoteUserId
	{
		get
		{
			Helper.Get(m_RemoteUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RemoteUserId);
		}
	}

	public SocketId? SocketId
	{
		get
		{
			Helper.Get<SocketIdInternal, SocketId>(m_SocketId, out SocketId? value);
			return value;
		}
		set
		{
			Helper.Set<SocketId, SocketIdInternal>(ref value, ref m_SocketId);
		}
	}

	public void Set(ref OnPeerConnectionInterruptedInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RemoteUserId = other.RemoteUserId;
		SocketId = other.SocketId;
	}

	public void Set(ref OnPeerConnectionInterruptedInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			RemoteUserId = other.Value.RemoteUserId;
			SocketId = other.Value.SocketId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RemoteUserId);
		Helper.Dispose(ref m_SocketId);
	}

	public void Get(out OnPeerConnectionInterruptedInfo output)
	{
		output = default(OnPeerConnectionInterruptedInfo);
		output.Set(ref this);
	}
}
