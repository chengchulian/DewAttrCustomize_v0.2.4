using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyInviteReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyInviteReceivedCallbackInfo>, ISettable<LobbyInviteReceivedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_InviteId;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

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

	public Utf8String InviteId
	{
		get
		{
			Helper.Get(m_InviteId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_InviteId);
		}
	}

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

	public ProductUserId TargetUserId
	{
		get
		{
			Helper.Get(m_TargetUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public void Set(ref LobbyInviteReceivedCallbackInfo other)
	{
		ClientData = other.ClientData;
		InviteId = other.InviteId;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref LobbyInviteReceivedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			InviteId = other.Value.InviteId;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_InviteId);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
	}

	public void Get(out LobbyInviteReceivedCallbackInfo output)
	{
		output = default(LobbyInviteReceivedCallbackInfo);
		output.Set(ref this);
	}
}
