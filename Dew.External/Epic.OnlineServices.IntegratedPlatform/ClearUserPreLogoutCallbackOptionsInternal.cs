using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ClearUserPreLogoutCallbackOptionsInternal : ISettable<ClearUserPreLogoutCallbackOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref ClearUserPreLogoutCallbackOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref ClearUserPreLogoutCallbackOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
		}
	}

	public void Dispose()
	{
	}
}
