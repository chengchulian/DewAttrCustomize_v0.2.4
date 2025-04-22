using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogGameRoundStartOptionsInternal : ISettable<LogGameRoundStartOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionIdentifier;

	private IntPtr m_LevelName;

	private IntPtr m_ModeName;

	private uint m_RoundTimeSeconds;

	public Utf8String SessionIdentifier
	{
		set
		{
			Helper.Set(value, ref m_SessionIdentifier);
		}
	}

	public Utf8String LevelName
	{
		set
		{
			Helper.Set(value, ref m_LevelName);
		}
	}

	public Utf8String ModeName
	{
		set
		{
			Helper.Set(value, ref m_ModeName);
		}
	}

	public uint RoundTimeSeconds
	{
		set
		{
			m_RoundTimeSeconds = value;
		}
	}

	public void Set(ref LogGameRoundStartOptions other)
	{
		m_ApiVersion = 1;
		SessionIdentifier = other.SessionIdentifier;
		LevelName = other.LevelName;
		ModeName = other.ModeName;
		RoundTimeSeconds = other.RoundTimeSeconds;
	}

	public void Set(ref LogGameRoundStartOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SessionIdentifier = other.Value.SessionIdentifier;
			LevelName = other.Value.LevelName;
			ModeName = other.Value.ModeName;
			RoundTimeSeconds = other.Value.RoundTimeSeconds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionIdentifier);
		Helper.Dispose(ref m_LevelName);
		Helper.Dispose(ref m_ModeName);
	}
}
