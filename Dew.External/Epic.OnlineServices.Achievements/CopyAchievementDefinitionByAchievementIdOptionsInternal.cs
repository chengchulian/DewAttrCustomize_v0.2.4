using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyAchievementDefinitionByAchievementIdOptionsInternal : ISettable<CopyAchievementDefinitionByAchievementIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AchievementId;

	public Utf8String AchievementId
	{
		set
		{
			Helper.Set(value, ref m_AchievementId);
		}
	}

	public void Set(ref CopyAchievementDefinitionByAchievementIdOptions other)
	{
		m_ApiVersion = 1;
		AchievementId = other.AchievementId;
	}

	public void Set(ref CopyAchievementDefinitionByAchievementIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AchievementId = other.Value.AchievementId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AchievementId);
	}
}
