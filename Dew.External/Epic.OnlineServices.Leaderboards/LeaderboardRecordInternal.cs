using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LeaderboardRecordInternal : IGettable<LeaderboardRecord>, ISettable<LeaderboardRecord>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private uint m_Rank;

	private int m_Score;

	private IntPtr m_UserDisplayName;

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

	public uint Rank
	{
		get
		{
			return m_Rank;
		}
		set
		{
			m_Rank = value;
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

	public Utf8String UserDisplayName
	{
		get
		{
			Helper.Get(m_UserDisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UserDisplayName);
		}
	}

	public void Set(ref LeaderboardRecord other)
	{
		m_ApiVersion = 2;
		UserId = other.UserId;
		Rank = other.Rank;
		Score = other.Score;
		UserDisplayName = other.UserDisplayName;
	}

	public void Set(ref LeaderboardRecord? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			UserId = other.Value.UserId;
			Rank = other.Value.Rank;
			Score = other.Value.Score;
			UserDisplayName = other.Value.UserDisplayName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_UserDisplayName);
	}

	public void Get(out LeaderboardRecord output)
	{
		output = default(LeaderboardRecord);
		output.Set(ref this);
	}
}
