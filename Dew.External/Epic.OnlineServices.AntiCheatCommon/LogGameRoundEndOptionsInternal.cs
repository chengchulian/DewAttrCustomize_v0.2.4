using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogGameRoundEndOptionsInternal : ISettable<LogGameRoundEndOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_WinningTeamId;

	public uint WinningTeamId
	{
		set
		{
			m_WinningTeamId = value;
		}
	}

	public void Set(ref LogGameRoundEndOptions other)
	{
		m_ApiVersion = 1;
		WinningTeamId = other.WinningTeamId;
	}

	public void Set(ref LogGameRoundEndOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			WinningTeamId = other.Value.WinningTeamId;
		}
	}

	public void Dispose()
	{
	}
}
