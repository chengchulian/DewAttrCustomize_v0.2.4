using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SendCustomNativeInviteRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendCustomNativeInviteRequestedCallbackInfo>, ISettable<SendCustomNativeInviteRequestedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private ulong m_UiEventId;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetNativeAccountType;

	private IntPtr m_TargetUserNativeAccountId;

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

	public void Set(ref SendCustomNativeInviteRequestedCallbackInfo other)
	{
		ClientData = other.ClientData;
		UiEventId = other.UiEventId;
		LocalUserId = other.LocalUserId;
		TargetNativeAccountType = other.TargetNativeAccountType;
		TargetUserNativeAccountId = other.TargetUserNativeAccountId;
		InviteId = other.InviteId;
	}

	public void Set(ref SendCustomNativeInviteRequestedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			UiEventId = other.Value.UiEventId;
			LocalUserId = other.Value.LocalUserId;
			TargetNativeAccountType = other.Value.TargetNativeAccountType;
			TargetUserNativeAccountId = other.Value.TargetUserNativeAccountId;
			InviteId = other.Value.InviteId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetNativeAccountType);
		Helper.Dispose(ref m_TargetUserNativeAccountId);
		Helper.Dispose(ref m_InviteId);
	}

	public void Get(out SendCustomNativeInviteRequestedCallbackInfo output)
	{
		output = default(SendCustomNativeInviteRequestedCallbackInfo);
		output.Set(ref this);
	}
}
