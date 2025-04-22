using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryLeaderboardUserScoresOptionsInternal : ISettable<QueryLeaderboardUserScoresOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserIds;

	private uint m_UserIdsCount;

	private IntPtr m_StatInfo;

	private uint m_StatInfoCount;

	private long m_StartTime;

	private long m_EndTime;

	private IntPtr m_LocalUserId;

	public ProductUserId[] UserIds
	{
		set
		{
			Helper.Set(value, ref m_UserIds, out m_UserIdsCount);
		}
	}

	public UserScoresQueryStatInfo[] StatInfo
	{
		set
		{
			Helper.Set<UserScoresQueryStatInfo, UserScoresQueryStatInfoInternal>(ref value, ref m_StatInfo, out m_StatInfoCount);
		}
	}

	public DateTimeOffset? StartTime
	{
		set
		{
			Helper.Set(value, ref m_StartTime);
		}
	}

	public DateTimeOffset? EndTime
	{
		set
		{
			Helper.Set(value, ref m_EndTime);
		}
	}

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public void Set(ref QueryLeaderboardUserScoresOptions other)
	{
		m_ApiVersion = 2;
		UserIds = other.UserIds;
		StatInfo = other.StatInfo;
		StartTime = other.StartTime;
		EndTime = other.EndTime;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref QueryLeaderboardUserScoresOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			UserIds = other.Value.UserIds;
			StatInfo = other.Value.StatInfo;
			StartTime = other.Value.StartTime;
			EndTime = other.Value.EndTime;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserIds);
		Helper.Dispose(ref m_StatInfo);
		Helper.Dispose(ref m_LocalUserId);
	}
}
