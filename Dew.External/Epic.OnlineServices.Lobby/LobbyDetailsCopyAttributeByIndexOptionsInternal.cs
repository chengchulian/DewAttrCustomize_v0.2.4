using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyDetailsCopyAttributeByIndexOptionsInternal : ISettable<LobbyDetailsCopyAttributeByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_AttrIndex;

	public uint AttrIndex
	{
		set
		{
			m_AttrIndex = value;
		}
	}

	public void Set(ref LobbyDetailsCopyAttributeByIndexOptions other)
	{
		m_ApiVersion = 1;
		AttrIndex = other.AttrIndex;
	}

	public void Set(ref LobbyDetailsCopyAttributeByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AttrIndex = other.Value.AttrIndex;
		}
	}

	public void Dispose()
	{
	}
}
