using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UnlockedAchievementInternal : IGettable<UnlockedAchievement>, ISettable<UnlockedAchievement>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AchievementId;

	private long m_UnlockTime;

	public Utf8String AchievementId
	{
		get
		{
			Helper.Get(m_AchievementId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AchievementId);
		}
	}

	public DateTimeOffset? UnlockTime
	{
		get
		{
			Helper.Get(m_UnlockTime, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UnlockTime);
		}
	}

	public void Set(ref UnlockedAchievement other)
	{
		m_ApiVersion = 1;
		AchievementId = other.AchievementId;
		UnlockTime = other.UnlockTime;
	}

	public void Set(ref UnlockedAchievement? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AchievementId = other.Value.AchievementId;
			UnlockTime = other.Value.UnlockTime;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AchievementId);
	}

	public void Get(out UnlockedAchievement output)
	{
		output = default(UnlockedAchievement);
		output.Set(ref this);
	}
}
