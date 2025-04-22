using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogPlayerDespawnOptionsInternal : ISettable<LogPlayerDespawnOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_DespawnedPlayerHandle;

	public IntPtr DespawnedPlayerHandle
	{
		set
		{
			m_DespawnedPlayerHandle = value;
		}
	}

	public void Set(ref LogPlayerDespawnOptions other)
	{
		m_ApiVersion = 1;
		DespawnedPlayerHandle = other.DespawnedPlayerHandle;
	}

	public void Set(ref LogPlayerDespawnOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			DespawnedPlayerHandle = other.Value.DespawnedPlayerHandle;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_DespawnedPlayerHandle);
	}
}
