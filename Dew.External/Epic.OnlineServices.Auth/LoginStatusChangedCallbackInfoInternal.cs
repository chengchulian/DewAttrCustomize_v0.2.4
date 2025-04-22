using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LoginStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginStatusChangedCallbackInfo>, ISettable<LoginStatusChangedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private LoginStatus m_PrevStatus;

	private LoginStatus m_CurrentStatus;

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

	public EpicAccountId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public LoginStatus PrevStatus
	{
		get
		{
			return m_PrevStatus;
		}
		set
		{
			m_PrevStatus = value;
		}
	}

	public LoginStatus CurrentStatus
	{
		get
		{
			return m_CurrentStatus;
		}
		set
		{
			m_CurrentStatus = value;
		}
	}

	public void Set(ref LoginStatusChangedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		PrevStatus = other.PrevStatus;
		CurrentStatus = other.CurrentStatus;
	}

	public void Set(ref LoginStatusChangedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			PrevStatus = other.Value.PrevStatus;
			CurrentStatus = other.Value.CurrentStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
	}

	public void Get(out LoginStatusChangedCallbackInfo output)
	{
		output = default(LoginStatusChangedCallbackInfo);
		output.Set(ref this);
	}
}
