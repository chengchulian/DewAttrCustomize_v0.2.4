using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UpdateLobbyOptionsInternal : ISettable<UpdateLobbyOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LobbyModificationHandle;

	public LobbyModification LobbyModificationHandle
	{
		set
		{
			Helper.Set(value, ref m_LobbyModificationHandle);
		}
	}

	public void Set(ref UpdateLobbyOptions other)
	{
		m_ApiVersion = 1;
		LobbyModificationHandle = other.LobbyModificationHandle;
	}

	public void Set(ref UpdateLobbyOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LobbyModificationHandle = other.Value.LobbyModificationHandle;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LobbyModificationHandle);
	}
}
