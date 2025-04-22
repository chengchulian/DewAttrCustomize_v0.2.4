using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryLeaderboardRanksOptionsInternal : ISettable<QueryLeaderboardRanksOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LeaderboardId;

	private IntPtr m_LocalUserId;

	public Utf8String LeaderboardId
	{
		set
		{
			Helper.Set(value, ref m_LeaderboardId);
		}
	}

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public void Set(ref QueryLeaderboardRanksOptions other)
	{
		m_ApiVersion = 2;
		LeaderboardId = other.LeaderboardId;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref QueryLeaderboardRanksOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LeaderboardId = other.Value.LeaderboardId;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LeaderboardId);
		Helper.Dispose(ref m_LocalUserId);
	}
}
