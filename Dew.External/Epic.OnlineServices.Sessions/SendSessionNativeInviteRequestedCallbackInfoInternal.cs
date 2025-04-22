using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SendSessionNativeInviteRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendSessionNativeInviteRequestedCallbackInfo>, ISettable<SendSessionNativeInviteRequestedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private ulong m_UiEventId;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetNativeAccountType;

	private IntPtr m_TargetUserNativeAccountId;

	private IntPtr m_SessionId;

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

	public ulong UiEventId
	{
		get
		{
			return m_UiEventId;
		}
		set
		{
			m_UiEventId = value;
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

	public Utf8String TargetNativeAccountType
	{
		get
		{
			Helper.Get(m_TargetNativeAccountType, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TargetNativeAccountType);
		}
	}

	public Utf8String TargetUserNativeAccountId
	{
		get
		{
			Helper.Get(m_TargetUserNativeAccountId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TargetUserNativeAccountId);
		}
	}

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

	public void Set(ref SendSessionNativeInviteRequestedCallbackInfo other)
	{
		ClientData = other.ClientData;
		UiEventId = other.UiEventId;
		LocalUserId = other.LocalUserId;
		TargetNativeAccountType = other.TargetNativeAccountType;
		TargetUserNativeAccountId = other.TargetUserNativeAccountId;
		SessionId = other.SessionId;
	}

	public void Set(ref SendSessionNativeInviteRequestedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			UiEventId = other.Value.UiEventId;
			LocalUserId = other.Value.LocalUserId;
			TargetNativeAccountType = other.Value.TargetNativeAccountType;
			TargetUserNativeAccountId = other.Value.TargetUserNativeAccountId;
			SessionId = other.Value.SessionId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetNativeAccountType);
		Helper.Dispose(ref m_TargetUserNativeAccountId);
		Helper.Dispose(ref m_SessionId);
	}

	public void Get(out SendSessionNativeInviteRequestedCallbackInfo output)
	{
		output = default(SendSessionNativeInviteRequestedCallbackInfo);
		output.Set(ref this);
	}
}
