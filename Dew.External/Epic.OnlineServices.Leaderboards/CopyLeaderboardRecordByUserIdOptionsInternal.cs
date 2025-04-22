using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLeaderboardRecordByUserIdOptionsInternal : ISettable<CopyLeaderboardRecordByUserIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	public ProductUserId UserId
	{
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public void Set(ref CopyLeaderboardRecordByUserIdOptions other)
	{
		m_ApiVersion = 2;
		UserId = other.UserId;
	}

	public void Set(ref CopyLeaderboardRecordByUserIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			UserId = other.Value.UserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
	}
}
