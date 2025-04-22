using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyDetailsGetMemberByIndexOptionsInternal : ISettable<LobbyDetailsGetMemberByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_MemberIndex;

	public uint MemberIndex
	{
		set
		{
			m_MemberIndex = value;
		}
	}

	public void Set(ref LobbyDetailsGetMemberByIndexOptions other)
	{
		m_ApiVersion = 1;
		MemberIndex = other.MemberIndex;
	}

	public void Set(ref LobbyDetailsGetMemberByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			MemberIndex = other.Value.MemberIndex;
		}
	}

	public void Dispose()
	{
	}
}
