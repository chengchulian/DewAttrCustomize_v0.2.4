using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RegisterPlayersOptionsInternal : ISettable<RegisterPlayersOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_SessionName;

	private IntPtr m_PlayersToRegister;

	private uint m_PlayersToRegisterCount;

	public Utf8String SessionName
	{
		set
		{
			Helper.Set(value, ref m_SessionName);
		}
	}

	public ProductUserId[] PlayersToRegister
	{
		set
		{
			Helper.Set(value, ref m_PlayersToRegister, out m_PlayersToRegisterCount);
		}
	}

	public void Set(ref RegisterPlayersOptions other)
	{
		m_ApiVersion = 3;
		SessionName = other.SessionName;
		PlayersToRegister = other.PlayersToRegister;
	}

	public void Set(ref RegisterPlayersOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			SessionName = other.Value.SessionName;
			PlayersToRegister = other.Value.PlayersToRegister;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SessionName);
		Helper.Dispose(ref m_PlayersToRegister);
	}
}
