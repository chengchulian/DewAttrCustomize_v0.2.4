using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLeaderboardRecordByIndexOptionsInternal : ISettable<CopyLeaderboardRecordByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_LeaderboardRecordIndex;

	public uint LeaderboardRecordIndex
	{
		set
		{
			m_LeaderboardRecordIndex = value;
		}
	}

	public void Set(ref CopyLeaderboardRecordByIndexOptions other)
	{
		m_ApiVersion = 2;
		LeaderboardRecordIndex = other.LeaderboardRecordIndex;
	}

	public void Set(ref CopyLeaderboardRecordByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LeaderboardRecordIndex = other.Value.LeaderboardRecordIndex;
		}
	}

	public void Dispose()
	{
	}
}
