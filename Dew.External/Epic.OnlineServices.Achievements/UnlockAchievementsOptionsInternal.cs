using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UnlockAchievementsOptionsInternal : ISettable<UnlockAchievementsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private IntPtr m_AchievementIds;

	private uint m_AchievementsCount;

	public ProductUserId UserId
	{
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public Utf8String[] AchievementIds
	{
		set
		{
			Helper.Set(value, ref m_AchievementIds, isArrayItemAllocated: true, out m_AchievementsCount);
		}
	}

	public void Set(ref UnlockAchievementsOptions other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
		AchievementIds = other.AchievementIds;
	}

	public void Set(ref UnlockAchievementsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
			AchievementIds = other.Value.AchievementIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_AchievementIds);
	}
}
