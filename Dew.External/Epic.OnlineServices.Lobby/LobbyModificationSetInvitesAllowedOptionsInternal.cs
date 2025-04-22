using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyModificationSetInvitesAllowedOptionsInternal : ISettable<LobbyModificationSetInvitesAllowedOptions>, IDisposable
{
	private int m_ApiVersion;

	private int m_InvitesAllowed;

	public bool InvitesAllowed
	{
		set
		{
			Helper.Set(value, ref m_InvitesAllowed);
		}
	}

	public void Set(ref LobbyModificationSetInvitesAllowedOptions other)
	{
		m_ApiVersion = 1;
		InvitesAllowed = other.InvitesAllowed;
	}

	public void Set(ref LobbyModificationSetInvitesAllowedOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			InvitesAllowed = other.Value.InvitesAllowed;
		}
	}

	public void Dispose()
	{
	}
}
