using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetAchievementDefinitionCountOptionsInternal : ISettable<GetAchievementDefinitionCountOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref GetAchievementDefinitionCountOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref GetAchievementDefinitionCountOptions? other)
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
