using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetDesktopCrossplayStatusOptionsInternal : ISettable<GetDesktopCrossplayStatusOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref GetDesktopCrossplayStatusOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref GetDesktopCrossplayStatusOptions? other)
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
