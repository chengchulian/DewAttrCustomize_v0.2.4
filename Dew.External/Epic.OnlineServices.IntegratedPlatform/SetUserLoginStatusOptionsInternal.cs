using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetUserLoginStatusOptionsInternal : ISettable<SetUserLoginStatusOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PlatformType;

	private IntPtr m_LocalPlatformUserId;

	private LoginStatus m_CurrentLoginStatus;

	public Utf8String PlatformType
	{
		set
		{
			Helper.Set(value, ref m_PlatformType);
		}
	}

	public Utf8String LocalPlatformUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalPlatformUserId);
		}
	}

	public LoginStatus CurrentLoginStatus
	{
		set
		{
			m_CurrentLoginStatus = value;
		}
	}

	public void Set(ref SetUserLoginStatusOptions other)
	{
		m_ApiVersion = 1;
		PlatformType = other.PlatformType;
		LocalPlatformUserId = other.LocalPlatformUserId;
		CurrentLoginStatus = other.CurrentLoginStatus;
	}

	public void Set(ref SetUserLoginStatusOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PlatformType = other.Value.PlatformType;
			LocalPlatformUserId = other.Value.LocalPlatformUserId;
			CurrentLoginStatus = other.Value.CurrentLoginStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PlatformType);
		Helper.Dispose(ref m_LocalPlatformUserId);
	}
}
