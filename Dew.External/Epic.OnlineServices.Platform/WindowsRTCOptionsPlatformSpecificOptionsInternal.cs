using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct WindowsRTCOptionsPlatformSpecificOptionsInternal : IGettable<WindowsRTCOptionsPlatformSpecificOptions>, ISettable<WindowsRTCOptionsPlatformSpecificOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_XAudio29DllPath;

	public Utf8String XAudio29DllPath
	{
		get
		{
			Helper.Get(m_XAudio29DllPath, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_XAudio29DllPath);
		}
	}

	public void Set(ref WindowsRTCOptionsPlatformSpecificOptions other)
	{
		m_ApiVersion = 1;
		XAudio29DllPath = other.XAudio29DllPath;
	}

	public void Set(ref WindowsRTCOptionsPlatformSpecificOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			XAudio29DllPath = other.Value.XAudio29DllPath;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_XAudio29DllPath);
	}

	public void Get(out WindowsRTCOptionsPlatformSpecificOptions output)
	{
		output = default(WindowsRTCOptionsPlatformSpecificOptions);
		output.Set(ref this);
	}
}
