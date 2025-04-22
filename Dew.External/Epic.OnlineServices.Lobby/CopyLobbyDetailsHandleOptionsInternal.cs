using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLobbyDetailsHandleOptionsInternal : ISettable<CopyLobbyDetailsHandleOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LobbyId;

	private IntPtr m_LocalUserId;

	public Utf8String LobbyId
	{
		set
		{
			Helper.Set(value, ref m_LobbyId);
		}
	}

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public void Set(ref CopyLobbyDetailsHandleOptions other)
	{
		m_ApiVersion = 1;
		LobbyId = other.LobbyId;
		LocalUserId = other.LocalUserId;
	}

	public void Set(ref CopyLobbyDetailsHandleOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LobbyId = other.Value.LobbyId;
			LocalUserId = other.Value.LocalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LobbyId);
		Helper.Dispose(ref m_LocalUserId);
	}
}
