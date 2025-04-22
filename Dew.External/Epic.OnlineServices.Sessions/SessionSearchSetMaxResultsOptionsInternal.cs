using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionSearchSetMaxResultsOptionsInternal : ISettable<SessionSearchSetMaxResultsOptions>, IDisposable
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

	public void Set(ref SessionSearchSetMaxResultsOptions other)
	{
		m_ApiVersion = 1;
		MaxSearchResults = other.MaxSearchResults;
	}

	public void Set(ref SessionSearchSetMaxResultsOptions? other)
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
