using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ActiveSessionGetRegisteredPlayerByIndexOptionsInternal : ISettable<ActiveSessionGetRegisteredPlayerByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_PlayerIndex;

	public uint PlayerIndex
	{
		set
		{
			m_PlayerIndex = value;
		}
	}

	public void Set(ref ActiveSessionGetRegisteredPlayerByIndexOptions other)
	{
		m_ApiVersion = 1;
		PlayerIndex = other.PlayerIndex;
	}

	public void Set(ref ActiveSessionGetRegisteredPlayerByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PlayerIndex = other.Value.PlayerIndex;
		}
	}

	public void Dispose()
	{
	}
}
