using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogPlayerReviveOptionsInternal : ISettable<LogPlayerReviveOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_RevivedPlayerHandle;

	private IntPtr m_ReviverPlayerHandle;

	public IntPtr RevivedPlayerHandle
	{
		set
		{
			m_RevivedPlayerHandle = value;
		}
	}

	public IntPtr ReviverPlayerHandle
	{
		set
		{
			m_ReviverPlayerHandle = value;
		}
	}

	public void Set(ref LogPlayerReviveOptions other)
	{
		m_ApiVersion = 1;
		RevivedPlayerHandle = other.RevivedPlayerHandle;
		ReviverPlayerHandle = other.ReviverPlayerHandle;
	}

	public void Set(ref LogPlayerReviveOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			RevivedPlayerHandle = other.Value.RevivedPlayerHandle;
			ReviverPlayerHandle = other.Value.ReviverPlayerHandle;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_RevivedPlayerHandle);
		Helper.Dispose(ref m_ReviverPlayerHandle);
	}
}
