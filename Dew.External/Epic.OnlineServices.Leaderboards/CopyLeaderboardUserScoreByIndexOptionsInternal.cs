using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLeaderboardUserScoreByIndexOptionsInternal : ISettable<CopyLeaderboardUserScoreByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_LeaderboardUserScoreIndex;

	private IntPtr m_StatName;

	public uint LeaderboardUserScoreIndex
	{
		set
		{
			m_LeaderboardUserScoreIndex = value;
		}
	}

	public Utf8String StatName
	{
		set
		{
			Helper.Set(value, ref m_StatName);
		}
	}

	public void Set(ref CopyLeaderboardUserScoreByIndexOptions other)
	{
		m_ApiVersion = 1;
		LeaderboardUserScoreIndex = other.LeaderboardUserScoreIndex;
		StatName = other.StatName;
	}

	public void Set(ref CopyLeaderboardUserScoreByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LeaderboardUserScoreIndex = other.Value.LeaderboardUserScoreIndex;
			StatName = other.Value.StatName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_StatName);
	}
}
