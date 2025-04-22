using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetLeaderboardUserScoreCountOptionsInternal : ISettable<GetLeaderboardUserScoreCountOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_StatName;

	public Utf8String StatName
	{
		set
		{
			Helper.Set(value, ref m_StatName);
		}
	}

	public void Set(ref GetLeaderboardUserScoreCountOptions other)
	{
		m_ApiVersion = 1;
		StatName = other.StatName;
	}

	public void Set(ref GetLeaderboardUserScoreCountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			StatName = other.Value.StatName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_StatName);
	}
}
