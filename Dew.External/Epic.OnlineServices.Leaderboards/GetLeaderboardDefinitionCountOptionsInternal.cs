using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetLeaderboardDefinitionCountOptionsInternal : ISettable<GetLeaderboardDefinitionCountOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref GetLeaderboardDefinitionCountOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref GetLeaderboardDefinitionCountOptions? other)
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
