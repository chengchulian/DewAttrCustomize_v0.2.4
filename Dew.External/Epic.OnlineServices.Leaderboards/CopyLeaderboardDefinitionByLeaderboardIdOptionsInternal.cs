using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal : ISettable<CopyLeaderboardDefinitionByLeaderboardIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LeaderboardId;

	public Utf8String LeaderboardId
	{
		set
		{
			Helper.Set(value, ref m_LeaderboardId);
		}
	}

	public void Set(ref CopyLeaderboardDefinitionByLeaderboardIdOptions other)
	{
		m_ApiVersion = 1;
		LeaderboardId = other.LeaderboardId;
	}

	public void Set(ref CopyLeaderboardDefinitionByLeaderboardIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LeaderboardId = other.Value.LeaderboardId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LeaderboardId);
	}
}
