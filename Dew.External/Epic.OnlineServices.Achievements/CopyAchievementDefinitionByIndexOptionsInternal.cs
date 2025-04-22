using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyAchievementDefinitionByIndexOptionsInternal : ISettable<CopyAchievementDefinitionByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_AchievementIndex;

	public uint AchievementIndex
	{
		set
		{
			m_AchievementIndex = value;
		}
	}

	public void Set(ref CopyAchievementDefinitionByIndexOptions other)
	{
		m_ApiVersion = 1;
		AchievementIndex = other.AchievementIndex;
	}

	public void Set(ref CopyAchievementDefinitionByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AchievementIndex = other.Value.AchievementIndex;
		}
	}

	public void Dispose()
	{
	}
}
