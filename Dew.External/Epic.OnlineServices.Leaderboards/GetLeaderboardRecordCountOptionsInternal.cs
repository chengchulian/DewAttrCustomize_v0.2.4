using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetLeaderboardRecordCountOptionsInternal : ISettable<GetLeaderboardRecordCountOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref GetLeaderboardRecordCountOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref GetLeaderboardRecordCountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
		}
	}

	public void Dispose()
	{
	}
}
