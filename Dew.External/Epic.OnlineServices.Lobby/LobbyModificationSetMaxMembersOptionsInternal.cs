using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyModificationSetMaxMembersOptionsInternal : ISettable<LobbyModificationSetMaxMembersOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_MaxMembers;

	public uint MaxMembers
	{
		set
		{
			m_MaxMembers = value;
		}
	}

	public void Set(ref LobbyModificationSetMaxMembersOptions other)
	{
		m_ApiVersion = 1;
		MaxMembers = other.MaxMembers;
	}

	public void Set(ref LobbyModificationSetMaxMembersOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			MaxMembers = other.Value.MaxMembers;
		}
	}

	public void Dispose()
	{
	}
}
