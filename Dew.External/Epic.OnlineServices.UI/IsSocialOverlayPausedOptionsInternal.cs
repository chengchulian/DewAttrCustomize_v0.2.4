using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct IsSocialOverlayPausedOptionsInternal : ISettable<IsSocialOverlayPausedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref IsSocialOverlayPausedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref IsSocialOverlayPausedOptions? other)
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
