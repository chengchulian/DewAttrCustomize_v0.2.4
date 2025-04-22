using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetUnlockedAchievementCountOptionsInternal : ISettable<GetUnlockedAchievementCountOptions>, IDisposable
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

	public void Set(ref GetUnlockedAchievementCountOptions other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
	}

	public void Set(ref GetUnlockedAchievementCountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
	}
}
