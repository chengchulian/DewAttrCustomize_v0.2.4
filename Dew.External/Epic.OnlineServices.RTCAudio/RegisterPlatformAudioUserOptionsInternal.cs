using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RegisterPlatformAudioUserOptionsInternal : ISettable<RegisterPlatformAudioUserOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	public Utf8String UserId
	{
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public void Set(ref RegisterPlatformAudioUserOptions other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
	}

	public void Set(ref RegisterPlatformAudioUserOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
	}
}
