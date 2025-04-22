using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbySearchSetLobbyIdOptionsInternal : ISettable<LobbySearchSetLobbyIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LobbyId;

	public Utf8String LobbyId
	{
		set
		{
			Helper.Set(value, ref m_LobbyId);
		}
	}

	public void Set(ref LobbySearchSetLobbyIdOptions other)
	{
		m_ApiVersion = 1;
		LobbyId = other.LobbyId;
	}

	public void Set(ref LobbySearchSetLobbyIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LobbyId = other.Value.LobbyId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LobbyId);
	}
}
