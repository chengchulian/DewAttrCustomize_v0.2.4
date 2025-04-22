using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyModificationSetPermissionLevelOptionsInternal : ISettable<LobbyModificationSetPermissionLevelOptions>, IDisposable
{
	private int m_ApiVersion;

	private LobbyPermissionLevel m_PermissionLevel;

	public LobbyPermissionLevel PermissionLevel
	{
		set
		{
			m_PermissionLevel = value;
		}
	}

	public void Set(ref LobbyModificationSetPermissionLevelOptions other)
	{
		m_ApiVersion = 1;
		PermissionLevel = other.PermissionLevel;
	}

	public void Set(ref LobbyModificationSetPermissionLevelOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PermissionLevel = other.Value.PermissionLevel;
		}
	}

	public void Dispose()
	{
	}
}
