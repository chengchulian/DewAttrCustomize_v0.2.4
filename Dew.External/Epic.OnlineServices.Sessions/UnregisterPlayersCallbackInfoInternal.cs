using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UnregisterPlayersCallbackInfoInternal : ICallbackInfoInternal, IGettable<UnregisterPlayersCallbackInfo>, ISettable<UnregisterPlayersCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_UnregisteredPlayers;

	private uint m_UnregisteredPlayersCount;

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

	public ProductUserId[] UnregisteredPlayers
	{
		get
		{
			Helper.GetHandle<ProductUserId>(m_UnregisteredPlayers, out var value, m_UnregisteredPlayersCount);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UnregisteredPlayers, out m_UnregisteredPlayersCount);
		}
	}

	public void Set(ref UnregisterPlayersCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		UnregisteredPlayers = other.UnregisteredPlayers;
	}

	public void Set(ref UnregisterPlayersCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			UnregisteredPlayers = other.Value.UnregisteredPlayers;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_UnregisteredPlayers);
	}

	public void Get(out UnregisterPlayersCallbackInfo output)
	{
		output = default(UnregisterPlayersCallbackInfo);
		output.Set(ref this);
	}
}
