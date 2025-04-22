using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryLeaderboardDefinitionsOptionsInternal : ISettable<QueryLeaderboardDefinitionsOptions>, IDisposable
{
	private int m_ApiVersion;

	private long m_StartTime;

	private long m_EndTime;

	private IntPtr m_LocalUserId;

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

	public void Set(ref QueryLeaderboardDefinitionsOptions other)
	{
		m_ApiVersion = 2;
		StartTime = other.StartTime;
		EndTime = other.EndTime;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref QueryLeaderboardDefinitionsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			StartTime = other.Value.StartTime;
			EndTime = other.Value.EndTime;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
	}
}
