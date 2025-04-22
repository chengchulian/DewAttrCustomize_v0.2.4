using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLobbyDetailsHandleByInviteIdOptionsInternal : ISettable<CopyLobbyDetailsHandleByInviteIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_InviteId;

	public Utf8String InviteId
	{
		set
		{
			Helper.Set(value, ref m_InviteId);
		}
	}

	public void Set(ref CopyLobbyDetailsHandleByInviteIdOptions other)
	{
		m_ApiVersion = 1;
		InviteId = other.InviteId;
	}

	public void Set(ref CopyLobbyDetailsHandleByInviteIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			InviteId = other.Value.InviteId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_InviteId);
	}
}
