using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PrePresentOptionsInternal : ISettable<PrePresentOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PlatformSpecificData;

	public IntPtr PlatformSpecificData
	{
		set
		{
			m_PlatformSpecificData = value;
		}
	}

	public void Set(ref PrePresentOptions other)
	{
		m_ApiVersion = 1;
		PlatformSpecificData = other.PlatformSpecificData;
	}

	public void Set(ref PrePresentOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PlatformSpecificData = other.Value.PlatformSpecificData;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PlatformSpecificData);
	}
}
