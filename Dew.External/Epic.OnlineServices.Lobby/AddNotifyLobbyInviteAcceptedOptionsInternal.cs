using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyLobbyInviteAcceptedOptionsInternal : ISettable<AddNotifyLobbyInviteAcceptedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyLobbyInviteAcceptedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyLobbyInviteAcceptedOptions? other)
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
