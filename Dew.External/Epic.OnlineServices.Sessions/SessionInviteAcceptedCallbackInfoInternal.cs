using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionInviteAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SessionInviteAcceptedCallbackInfo>, ISettable<SessionInviteAcceptedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_SessionId;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	private IntPtr m_InviteId;

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

	public Utf8String SessionId
	{
		get
		{
			Helper.Get(m_SessionId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SessionId);
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

	public void Set(ref SessionInviteAcceptedCallbackInfo other)
	{
		ClientData = other.ClientData;
		SessionId = other.SessionId;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		InviteId = other.InviteId;
	}

	public void Set(ref SessionInviteAcceptedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			SessionId = other.Value.SessionId;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
			InviteId = other.Value.InviteId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_SessionId);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_InviteId);
	}

	public void Get(out SessionInviteAcceptedCallbackInfo output)
	{
		output = default(SessionInviteAcceptedCallbackInfo);
		output.Set(ref this);
	}
}
