using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetGameSessionIdOptionsInternal : ISettable<SetGameSessionIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_GameSessionId;

	public Utf8String GameSessionId
	{
		set
		{
			Helper.Set(value, ref m_GameSessionId);
		}
	}

	public void Set(ref SetGameSessionIdOptions other)
	{
		m_ApiVersion = 1;
		GameSessionId = other.GameSessionId;
	}

	public void Set(ref SetGameSessionIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			GameSessionId = other.Value.GameSessionId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_GameSessionId);
	}
}
