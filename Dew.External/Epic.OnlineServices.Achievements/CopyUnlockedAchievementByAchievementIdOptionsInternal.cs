using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyUnlockedAchievementByAchievementIdOptionsInternal : ISettable<CopyUnlockedAchievementByAchievementIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private IntPtr m_AchievementId;

	public ProductUserId UserId
	{
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public Utf8String AchievementId
	{
		set
		{
			Helper.Set(value, ref m_AchievementId);
		}
	}

	public void Set(ref CopyUnlockedAchievementByAchievementIdOptions other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
		AchievementId = other.AchievementId;
	}

	public void Set(ref CopyUnlockedAchievementByAchievementIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
			AchievementId = other.Value.AchievementId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_AchievementId);
	}
}
