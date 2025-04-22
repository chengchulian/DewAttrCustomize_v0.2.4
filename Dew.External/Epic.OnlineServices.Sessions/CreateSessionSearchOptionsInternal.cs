using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CreateSessionSearchOptionsInternal : ISettable<CreateSessionSearchOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_MaxSearchResults;

	public uint MaxSearchResults
	{
		set
		{
			m_MaxSearchResults = value;
		}
	}

	public void Set(ref CreateSessionSearchOptions other)
	{
		m_ApiVersion = 1;
		MaxSearchResults = other.MaxSearchResults;
	}

	public void Set(ref CreateSessionSearchOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			MaxSearchResults = other.Value.MaxSearchResults;
		}
	}

	public void Dispose()
	{
	}
}
