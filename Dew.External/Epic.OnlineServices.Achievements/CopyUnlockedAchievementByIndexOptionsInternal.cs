using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyUnlockedAchievementByIndexOptionsInternal : ISettable<CopyUnlockedAchievementByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_UserId;

	private uint m_AchievementIndex;

	public ProductUserId UserId
	{
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public uint AchievementIndex
	{
		set
		{
			m_AchievementIndex = value;
		}
	}

	public void Set(ref CopyUnlockedAchievementByIndexOptions other)
	{
		m_ApiVersion = 1;
		UserId = other.UserId;
		AchievementIndex = other.AchievementIndex;
	}

	public void Set(ref CopyUnlockedAchievementByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UserId = other.Value.UserId;
			AchievementIndex = other.Value.AchievementIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
	}
}
