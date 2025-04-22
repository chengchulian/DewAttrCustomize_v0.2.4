using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnCustomInviteAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnCustomInviteAcceptedCallbackInfo>, ISettable<OnCustomInviteAcceptedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_TargetUserId;

	private IntPtr m_LocalUserId;

	private IntPtr m_CustomInviteId;

	private IntPtr m_Payload;

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

	public Utf8String CustomInviteId
	{
		get
		{
			Helper.Get(m_CustomInviteId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_CustomInviteId);
		}
	}

	public Utf8String Payload
	{
		get
		{
			Helper.Get(m_Payload, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Payload);
		}
	}

	public void Set(ref OnCustomInviteAcceptedCallbackInfo other)
	{
		ClientData = other.ClientData;
		TargetUserId = other.TargetUserId;
		LocalUserId = other.LocalUserId;
		CustomInviteId = other.CustomInviteId;
		Payload = other.Payload;
	}

	public void Set(ref OnCustomInviteAcceptedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			TargetUserId = other.Value.TargetUserId;
			LocalUserId = other.Value.LocalUserId;
			CustomInviteId = other.Value.CustomInviteId;
			Payload = other.Value.Payload;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_CustomInviteId);
		Helper.Dispose(ref m_Payload);
	}

	public void Get(out OnCustomInviteAcceptedCallbackInfo output)
	{
		output = default(OnCustomInviteAcceptedCallbackInfo);
		output.Set(ref this);
	}
}
