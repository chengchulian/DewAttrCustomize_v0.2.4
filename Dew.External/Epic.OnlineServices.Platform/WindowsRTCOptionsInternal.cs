using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct WindowsRTCOptionsInternal : IGettable<WindowsRTCOptions>, ISettable<WindowsRTCOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PlatformSpecificOptions;

	private RTCBackgroundMode m_BackgroundMode;

	public WindowsRTCOptionsPlatformSpecificOptions? PlatformSpecificOptions
	{
		get
		{
			Helper.Get<WindowsRTCOptionsPlatformSpecificOptionsInternal, WindowsRTCOptionsPlatformSpecificOptions>(m_PlatformSpecificOptions, out WindowsRTCOptionsPlatformSpecificOptions? value);
			return value;
		}
		set
		{
			Helper.Set<WindowsRTCOptionsPlatformSpecificOptions, WindowsRTCOptionsPlatformSpecificOptionsInternal>(ref value, ref m_PlatformSpecificOptions);
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

	public void Set(ref WindowsRTCOptions other)
	{
		m_ApiVersion = 2;
		PlatformSpecificOptions = other.PlatformSpecificOptions;
		BackgroundMode = other.BackgroundMode;
	}

	public void Set(ref WindowsRTCOptions? other)
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

	public void Get(out WindowsRTCOptions output)
	{
		output = default(WindowsRTCOptions);
		output.Set(ref this);
	}
}
