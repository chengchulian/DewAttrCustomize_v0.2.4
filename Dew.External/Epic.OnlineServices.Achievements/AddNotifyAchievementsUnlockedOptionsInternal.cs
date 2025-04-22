using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyAchievementsUnlockedOptionsInternal : ISettable<AddNotifyAchievementsUnlockedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyAchievementsUnlockedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyAchievementsUnlockedOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
		}
	}

	public void Dispose()
	{
	}
}
