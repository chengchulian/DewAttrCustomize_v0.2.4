using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct FinalizeDeferredUserLogoutOptionsInternal : ISettable<FinalizeDeferredUserLogoutOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PlatformType;

	private IntPtr m_LocalPlatformUserId;

	private LoginStatus m_ExpectedLoginStatus;

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

	public LoginStatus ExpectedLoginStatus
	{
		set
		{
			m_ExpectedLoginStatus = value;
		}
	}

	public void Set(ref FinalizeDeferredUserLogoutOptions other)
	{
		m_ApiVersion = 1;
		PlatformType = other.PlatformType;
		LocalPlatformUserId = other.LocalPlatformUserId;
		ExpectedLoginStatus = other.ExpectedLoginStatus;
	}

	public void Set(ref FinalizeDeferredUserLogoutOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PlatformType = other.Value.PlatformType;
			LocalPlatformUserId = other.Value.LocalPlatformUserId;
			ExpectedLoginStatus = other.Value.ExpectedLoginStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PlatformType);
		Helper.Dispose(ref m_LocalPlatformUserId);
	}
}
