using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PauseSocialOverlayOptionsInternal : ISettable<PauseSocialOverlayOptions>, IDisposable
{
	private int m_ApiVersion;

	private int m_IsPaused;

	public bool IsPaused
	{
		set
		{
			Helper.Set(value, ref m_IsPaused);
		}
	}

	public void Set(ref PauseSocialOverlayOptions other)
	{
		m_ApiVersion = 1;
		IsPaused = other.IsPaused;
	}

	public void Set(ref PauseSocialOverlayOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			IsPaused = other.Value.IsPaused;
		}
	}

	public void Dispose()
	{
	}
}
