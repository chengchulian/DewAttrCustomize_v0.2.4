using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyPlayerAchievementByAchievementIdOptionsInternal : ISettable<CopyPlayerAchievementByAchievementIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private IntPtr m_AchievementId;

	private IntPtr m_LocalUserId;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public Utf8String AchievementId
	{
		set
		{
			Helper.Set(value, ref m_AchievementId);
		}
	}

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public void Set(ref CopyPlayerAchievementByAchievementIdOptions other)
	{
		m_ApiVersion = 2;
		TargetUserId = other.TargetUserId;
		AchievementId = other.AchievementId;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref CopyPlayerAchievementByAchievementIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			TargetUserId = other.Value.TargetUserId;
			AchievementId = other.Value.AchievementId;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_AchievementId);
		Helper.Dispose(ref m_LocalUserId);
	}
}
