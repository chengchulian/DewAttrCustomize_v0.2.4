using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RegisterPlayersCallbackInfoInternal : ICallbackInfoInternal, IGettable<RegisterPlayersCallbackInfo>, ISettable<RegisterPlayersCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_RegisteredPlayers;

	private uint m_RegisteredPlayersCount;

	private IntPtr m_SanctionedPlayers;

	private uint m_SanctionedPlayersCount;

	public Result ResultCode
	{
		get
		{
			return m_ResultCode;
		}
		set
		{
			m_ResultCode = value;
		}
	}

	public object ClientData
	{
		get
		{
			Helper.Get(m_ClientData, out object value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientData);
		}
	}

	public IntPtr ClientDataAddress => m_ClientData;

	public ProductUserId[] RegisteredPlayers
	{
		get
		{
			Helper.GetHandle<ProductUserId>(m_RegisteredPlayers, out var value, m_RegisteredPlayersCount);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RegisteredPlayers, out m_RegisteredPlayersCount);
		}
	}

	public ProductUserId[] SanctionedPlayers
	{
		get
		{
			Helper.GetHandle<ProductUserId>(m_SanctionedPlayers, out var value, m_SanctionedPlayersCount);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SanctionedPlayers, out m_SanctionedPlayersCount);
		}
	}

	public void Set(ref RegisterPlayersCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		RegisteredPlayers = other.RegisteredPlayers;
		SanctionedPlayers = other.SanctionedPlayers;
	}

	public void Set(ref RegisterPlayersCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			RegisteredPlayers = other.Value.RegisteredPlayers;
			SanctionedPlayers = other.Value.SanctionedPlayers;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_RegisteredPlayers);
		Helper.Dispose(ref m_SanctionedPlayers);
	}

	public void Get(out RegisterPlayersCallbackInfo output)
	{
		output = default(RegisterPlayersCallbackInfo);
		output.Set(ref this);
	}
}
