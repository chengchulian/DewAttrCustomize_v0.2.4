using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LeaveSessionRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LeaveSessionRequestedCallbackInfo>, ISettable<LeaveSessionRequestedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_SessionName;

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

	public Utf8String SessionName
	{
		get
		{
			Helper.Get(m_SessionName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SessionName);
		}
	}

	public void Set(ref LeaveSessionRequestedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		SessionName = other.SessionName;
	}

	public void Set(ref LeaveSessionRequestedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			SessionName = other.Value.SessionName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_SessionName);
	}

	public void Get(out LeaveSessionRequestedCallbackInfo output)
	{
		output = default(LeaveSessionRequestedCallbackInfo);
		output.Set(ref this);
	}
}
