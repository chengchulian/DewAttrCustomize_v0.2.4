using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UserScoresQueryStatInfoInternal : IGettable<UserScoresQueryStatInfo>, ISettable<UserScoresQueryStatInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_StatName;

	private LeaderboardAggregation m_Aggregation;

	public Utf8String StatName
	{
		get
		{
			Helper.Get(m_StatName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_StatName);
		}
	}

	public LeaderboardAggregation Aggregation
	{
		get
		{
			return m_Aggregation;
		}
		set
		{
			m_Aggregation = value;
		}
	}

	public void Set(ref UserScoresQueryStatInfo other)
	{
		m_ApiVersion = 1;
		StatName = other.StatName;
		Aggregation = other.Aggregation;
	}

	public void Set(ref UserScoresQueryStatInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			StatName = other.Value.StatName;
			Aggregation = other.Value.Aggregation;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_StatName);
	}

	public void Get(out UserScoresQueryStatInfo output)
	{
		output = default(UserScoresQueryStatInfo);
		output.Set(ref this);
	}
}
