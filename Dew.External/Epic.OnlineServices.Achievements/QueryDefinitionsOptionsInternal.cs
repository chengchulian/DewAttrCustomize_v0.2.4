using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryDefinitionsOptionsInternal : ISettable<QueryDefinitionsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_EpicUserId_DEPRECATED;

	private IntPtr m_HiddenAchievementIds_DEPRECATED;

	private uint m_HiddenAchievementsCount_DEPRECATED;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public EpicAccountId EpicUserId_DEPRECATED
	{
		set
		{
			Helper.Set(value, ref m_EpicUserId_DEPRECATED);
		}
	}

	public Utf8String[] HiddenAchievementIds_DEPRECATED
	{
		set
		{
			Helper.Set(value, ref m_HiddenAchievementIds_DEPRECATED, isArrayItemAllocated: true, out m_HiddenAchievementsCount_DEPRECATED);
		}
	}

	public void Set(ref QueryDefinitionsOptions other)
	{
		m_ApiVersion = 3;
		LocalUserId = other.LocalUserId;
		EpicUserId_DEPRECATED = other.EpicUserId_DEPRECATED;
		HiddenAchievementIds_DEPRECATED = other.HiddenAchievementIds_DEPRECATED;
	}

	public void Set(ref QueryDefinitionsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			LocalUserId = other.Value.LocalUserId;
			EpicUserId_DEPRECATED = other.Value.EpicUserId_DEPRECATED;
			HiddenAchievementIds_DEPRECATED = other.Value.HiddenAchievementIds_DEPRECATED;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_EpicUserId_DEPRECATED);
		Helper.Dispose(ref m_HiddenAchievementIds_DEPRECATED);
	}
}
