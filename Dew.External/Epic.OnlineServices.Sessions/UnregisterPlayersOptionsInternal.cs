using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UnregisterPlayersOptionsInternal : ISettable<UnregisterPlayersOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionName;

	private IntPtr m_PlayersToUnregister;

	private uint m_PlayersToUnregisterCount;

	public Utf8String SessionName
	{
		set
		{
			Helper.Set(value, ref m_SessionName);
		}
	}

	public ProductUserId[] PlayersToUnregister
	{
		set
		{
			Helper.Set(value, ref m_PlayersToUnregister, out m_PlayersToUnregisterCount);
		}
	}

	public void Set(ref UnregisterPlayersOptions other)
	{
		m_ApiVersion = 2;
		SessionName = other.SessionName;
		PlayersToUnregister = other.PlayersToUnregister;
	}

	public void Set(ref UnregisterPlayersOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			SessionName = other.Value.SessionName;
			PlayersToUnregister = other.Value.PlayersToUnregister;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionName);
		Helper.Dispose(ref m_PlayersToUnregister);
	}
}
