using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyModificationRemoveAttributeOptionsInternal : ISettable<LobbyModificationRemoveAttributeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Key;

	public Utf8String Key
	{
		set
		{
			Helper.Set(value, ref m_Key);
		}
	}

	public void Set(ref LobbyModificationRemoveAttributeOptions other)
	{
		m_ApiVersion = 1;
		Key = other.Key;
	}

	public void Set(ref LobbyModificationRemoveAttributeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Key = other.Value.Key;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Key);
	}
}
