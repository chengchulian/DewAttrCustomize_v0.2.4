using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLeaderboardUserScoreByUserIdOptionsInternal : ISettable<CopyLeaderboardUserScoreByUserIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private IntPtr m_StatName;

	public ProductUserId UserId
	{
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public Utf8String StatName
	{
		set
		{
			Helper.Set(value, ref m_StatName);
		}
	}

	public void Set(ref CopyLeaderboardUserScoreByUserIdOptions other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
		StatName = other.StatName;
	}

	public void Set(ref CopyLeaderboardUserScoreByUserIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
			StatName = other.Value.StatName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_StatName);
	}
}
