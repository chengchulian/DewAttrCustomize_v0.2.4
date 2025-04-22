using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyDetailsCopyAttributeByKeyOptionsInternal : ISettable<LobbyDetailsCopyAttributeByKeyOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_AttrKey;

	public Utf8String AttrKey
	{
		set
		{
			Helper.Set(value, ref m_AttrKey);
		}
	}

	public void Set(ref LobbyDetailsCopyAttributeByKeyOptions other)
	{
		m_ApiVersion = 1;
		AttrKey = other.AttrKey;
	}

	public void Set(ref LobbyDetailsCopyAttributeByKeyOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AttrKey = other.Value.AttrKey;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AttrKey);
	}
}
