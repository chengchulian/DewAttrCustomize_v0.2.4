using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLobbyDetailsHandleByUiEventIdOptionsInternal : ISettable<CopyLobbyDetailsHandleByUiEventIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private ulong m_UiEventId;

	public ulong UiEventId
	{
		set
		{
			m_UiEventId = value;
		}
	}

	public void Set(ref CopyLobbyDetailsHandleByUiEventIdOptions other)
	{
		m_ApiVersion = 1;
		UiEventId = other.UiEventId;
	}

	public void Set(ref CopyLobbyDetailsHandleByUiEventIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			UiEventId = other.Value.UiEventId;
		}
	}

	public void Dispose()
	{
	}
}
