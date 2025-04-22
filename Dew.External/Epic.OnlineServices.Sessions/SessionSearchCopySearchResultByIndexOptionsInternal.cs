using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionSearchCopySearchResultByIndexOptionsInternal : ISettable<SessionSearchCopySearchResultByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_SessionIndex;

	public uint SessionIndex
	{
		set
		{
			m_SessionIndex = value;
		}
	}

	public void Set(ref SessionSearchCopySearchResultByIndexOptions other)
	{
		m_ApiVersion = 1;
		SessionIndex = other.SessionIndex;
	}

	public void Set(ref SessionSearchCopySearchResultByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			SessionIndex = other.Value.SessionIndex;
		}
	}

	public void Dispose()
	{
	}
}
