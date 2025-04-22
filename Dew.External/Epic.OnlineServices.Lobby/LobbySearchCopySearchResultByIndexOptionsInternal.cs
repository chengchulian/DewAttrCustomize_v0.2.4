using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbySearchCopySearchResultByIndexOptionsInternal : ISettable<LobbySearchCopySearchResultByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_LobbyIndex;

	public uint LobbyIndex
	{
		set
		{
			m_LobbyIndex = value;
		}
	}

	public void Set(ref LobbySearchCopySearchResultByIndexOptions other)
	{
		m_ApiVersion = 1;
		LobbyIndex = other.LobbyIndex;
	}

	public void Set(ref LobbySearchCopySearchResultByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LobbyIndex = other.Value.LobbyIndex;
		}
	}

	public void Dispose()
	{
	}
}
