using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLeaderboardDefinitionByIndexOptionsInternal : ISettable<CopyLeaderboardDefinitionByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_LeaderboardIndex;

	public uint LeaderboardIndex
	{
		set
		{
			m_LeaderboardIndex = value;
		}
	}

	public void Set(ref CopyLeaderboardDefinitionByIndexOptions other)
	{
		m_ApiVersion = 1;
		LeaderboardIndex = other.LeaderboardIndex;
	}

	public void Set(ref CopyLeaderboardDefinitionByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LeaderboardIndex = other.Value.LeaderboardIndex;
		}
	}

	public void Dispose()
	{
	}
}
