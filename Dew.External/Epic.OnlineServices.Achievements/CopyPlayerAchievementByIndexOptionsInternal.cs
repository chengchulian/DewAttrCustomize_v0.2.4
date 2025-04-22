using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyPlayerAchievementByIndexOptionsInternal : ISettable<CopyPlayerAchievementByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private uint m_AchievementIndex;

	private IntPtr m_LocalUserId;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public uint AchievementIndex
	{
		set
		{
			m_AchievementIndex = value;
		}
	}

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public void Set(ref CopyPlayerAchievementByIndexOptions other)
	{
		m_ApiVersion = 2;
		TargetUserId = other.TargetUserId;
		AchievementIndex = other.AchievementIndex;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref CopyPlayerAchievementByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			TargetUserId = other.Value.TargetUserId;
			AchievementIndex = other.Value.AchievementIndex;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_LocalUserId);
	}
}
