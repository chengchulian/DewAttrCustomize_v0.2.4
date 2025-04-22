using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyDetailsGetAttributeCountOptionsInternal : ISettable<LobbyDetailsGetAttributeCountOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref LobbyDetailsGetAttributeCountOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref LobbyDetailsGetAttributeCountOptions? other)
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
