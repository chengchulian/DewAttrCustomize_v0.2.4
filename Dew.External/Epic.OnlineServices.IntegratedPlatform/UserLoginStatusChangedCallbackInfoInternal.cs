using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UserLoginStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<UserLoginStatusChangedCallbackInfo>, ISettable<UserLoginStatusChangedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_PlatformType;

	private IntPtr m_LocalPlatformUserId;

	private IntPtr m_AccountId;

	private IntPtr m_ProductUserId;

	private LoginStatus m_PreviousLoginStatus;

	private LoginStatus m_CurrentLoginStatus;

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

	public Utf8String PlatformType
	{
		get
		{
			Helper.Get(m_PlatformType, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_PlatformType);
		}
	}

	public Utf8String LocalPlatformUserId
	{
		get
		{
			Helper.Get(m_LocalPlatformUserId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalPlatformUserId);
		}
	}

	public EpicAccountId AccountId
	{
		get
		{
			Helper.Get(m_AccountId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AccountId);
		}
	}

	public ProductUserId ProductUserId
	{
		get
		{
			Helper.Get(m_ProductUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ProductUserId);
		}
	}

	public LoginStatus PreviousLoginStatus
	{
		get
		{
			return m_PreviousLoginStatus;
		}
		set
		{
			m_PreviousLoginStatus = value;
		}
	}

	public LoginStatus CurrentLoginStatus
	{
		get
		{
			return m_CurrentLoginStatus;
		}
		set
		{
			m_CurrentLoginStatus = value;
		}
	}

	public void Set(ref UserLoginStatusChangedCallbackInfo other)
	{
		ClientData = other.ClientData;
		PlatformType = other.PlatformType;
		LocalPlatformUserId = other.LocalPlatformUserId;
		AccountId = other.AccountId;
		ProductUserId = other.ProductUserId;
		PreviousLoginStatus = other.PreviousLoginStatus;
		CurrentLoginStatus = other.CurrentLoginStatus;
	}

	public void Set(ref UserLoginStatusChangedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			PlatformType = other.Value.PlatformType;
			LocalPlatformUserId = other.Value.LocalPlatformUserId;
			AccountId = other.Value.AccountId;
			ProductUserId = other.Value.ProductUserId;
			PreviousLoginStatus = other.Value.PreviousLoginStatus;
			CurrentLoginStatus = other.Value.CurrentLoginStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_PlatformType);
		Helper.Dispose(ref m_LocalPlatformUserId);
		Helper.Dispose(ref m_AccountId);
		Helper.Dispose(ref m_ProductUserId);
	}

	public void Get(out UserLoginStatusChangedCallbackInfo output)
	{
		output = default(UserLoginStatusChangedCallbackInfo);
		output.Set(ref this);
	}
}
