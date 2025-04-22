using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UnregisterPlatformUserOptionsInternal : ISettable<UnregisterPlatformUserOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PlatformUserId;

	public Utf8String PlatformUserId
	{
		set
		{
			Helper.Set(value, ref m_PlatformUserId);
		}
	}

	public void Set(ref UnregisterPlatformUserOptions other)
	{
		m_ApiVersion = 1;
		PlatformUserId = other.PlatformUserId;
	}

	public void Set(ref UnregisterPlatformUserOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PlatformUserId = other.Value.PlatformUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PlatformUserId);
	}
}
