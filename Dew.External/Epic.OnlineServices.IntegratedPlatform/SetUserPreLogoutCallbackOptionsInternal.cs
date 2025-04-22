using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetUserPreLogoutCallbackOptionsInternal : ISettable<SetUserPreLogoutCallbackOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref SetUserPreLogoutCallbackOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref SetUserPreLogoutCallbackOptions? other)
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
