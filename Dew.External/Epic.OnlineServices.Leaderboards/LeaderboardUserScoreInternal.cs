using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LeaderboardUserScoreInternal : IGettable<LeaderboardUserScore>, ISettable<LeaderboardUserScore>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private int m_Score;

	public ProductUserId UserId
	{
		get
		{
			Helper.Get(m_UserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public int Score
	{
		get
		{
			return m_Score;
		}
		set
		{
			m_Score = value;
		}
	}

	public void Set(ref LeaderboardUserScore other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
		Score = other.Score;
	}

	public void Set(ref LeaderboardUserScore? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
			Score = other.Value.Score;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
	}

	public void Get(out LeaderboardUserScore output)
	{
		output = default(LeaderboardUserScore);
		output.Set(ref this);
	}
}
