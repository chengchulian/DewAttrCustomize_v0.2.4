using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationSetAllowedPlatformIdsOptionsInternal : ISettable<SessionModificationSetAllowedPlatformIdsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AllowedPlatformIds;

	private uint m_AllowedPlatformIdsCount;

	public uint[] AllowedPlatformIds
	{
		set
		{
			Helper.Set(value, ref m_AllowedPlatformIds, out m_AllowedPlatformIdsCount);
		}
	}

	public void Set(ref SessionModificationSetAllowedPlatformIdsOptions other)
	{
		m_ApiVersion = 1;
		AllowedPlatformIds = other.AllowedPlatformIds;
	}

	public void Set(ref SessionModificationSetAllowedPlatformIdsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AllowedPlatformIds = other.Value.AllowedPlatformIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AllowedPlatformIds);
	}
}
