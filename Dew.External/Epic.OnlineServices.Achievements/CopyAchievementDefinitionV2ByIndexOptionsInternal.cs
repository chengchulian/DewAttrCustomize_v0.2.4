using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyAchievementDefinitionV2ByIndexOptionsInternal : ISettable<CopyAchievementDefinitionV2ByIndexOptions>, IDisposable
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

	public void Set(ref CopyAchievementDefinitionV2ByIndexOptions other)
	{
		m_ApiVersion = 2;
		AchievementIndex = other.AchievementIndex;
	}

	public void Set(ref CopyAchievementDefinitionV2ByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			AchievementIndex = other.Value.AchievementIndex;
		}
	}

	public void Dispose()
	{
	}
}
