using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RTCOptionsInternal : IGettable<RTCOptions>, ISettable<RTCOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PlatformSpecificOptions;

	private RTCBackgroundMode m_BackgroundMode;

	public IntPtr PlatformSpecificOptions
	{
		get
		{
			return m_PlatformSpecificOptions;
		}
		set
		{
			m_PlatformSpecificOptions = value;
		}
	}

	public RTCBackgroundMode BackgroundMode
	{
		get
		{
			return m_BackgroundMode;
		}
		set
		{
			m_BackgroundMode = value;
		}
	}

	public void Set(ref RTCOptions other)
	{
		m_ApiVersion = 2;
		PlatformSpecificOptions = other.PlatformSpecificOptions;
		BackgroundMode = other.BackgroundMode;
	}

	public void Set(ref RTCOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			PlatformSpecificOptions = other.Value.PlatformSpecificOptions;
			BackgroundMode = other.Value.BackgroundMode;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PlatformSpecificOptions);
	}

	public void Get(out RTCOptions output)
	{
		output = default(RTCOptions);
		output.Set(ref this);
	}
}
