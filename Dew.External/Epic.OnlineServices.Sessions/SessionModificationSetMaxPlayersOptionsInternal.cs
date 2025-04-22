using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationSetMaxPlayersOptionsInternal : ISettable<SessionModificationSetMaxPlayersOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_MaxPlayers;

	public uint MaxPlayers
	{
		set
		{
			m_MaxPlayers = value;
		}
	}

	public void Set(ref SessionModificationSetMaxPlayersOptions other)
	{
		m_ApiVersion = 1;
		MaxPlayers = other.MaxPlayers;
	}

	public void Set(ref SessionModificationSetMaxPlayersOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			MaxPlayers = other.Value.MaxPlayers;
		}
	}

	public void Dispose()
	{
	}
}
