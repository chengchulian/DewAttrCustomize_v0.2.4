using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyRTCRoomConnectionChangedOptionsInternal : ISettable<AddNotifyRTCRoomConnectionChangedOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LobbyId_DEPRECATED;

	private IntPtr m_LocalUserId_DEPRECATED;

	public Utf8String LobbyId_DEPRECATED
	{
		set
		{
			Helper.Set(value, ref m_LobbyId_DEPRECATED);
		}
	}

	public ProductUserId LocalUserId_DEPRECATED
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId_DEPRECATED);
		}
	}

	public void Set(ref AddNotifyRTCRoomConnectionChangedOptions other)
	{
		m_ApiVersion = 2;
		LobbyId_DEPRECATED = other.LobbyId_DEPRECATED;
		LocalUserId_DEPRECATED = other.LocalUserId_DEPRECATED;
	}

	public void Set(ref AddNotifyRTCRoomConnectionChangedOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LobbyId_DEPRECATED = other.Value.LobbyId_DEPRECATED;
			LocalUserId_DEPRECATED = other.Value.LocalUserId_DEPRECATED;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LobbyId_DEPRECATED);
		Helper.Dispose(ref m_LocalUserId_DEPRECATED);
	}
}
