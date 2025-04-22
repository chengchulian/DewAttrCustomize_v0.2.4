using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LobbyUpdateReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyUpdateReceivedCallbackInfo>, ISettable<LobbyUpdateReceivedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LobbyId;

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

	public Utf8String LobbyId
	{
		get
		{
			Helper.Get(m_LobbyId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LobbyId);
		}
	}

	public void Set(ref LobbyUpdateReceivedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LobbyId = other.LobbyId;
	}

	public void Set(ref LobbyUpdateReceivedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LobbyId = other.Value.LobbyId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LobbyId);
	}

	public void Get(out LobbyUpdateReceivedCallbackInfo output)
	{
		output = default(LobbyUpdateReceivedCallbackInfo);
		output.Set(ref this);
	}
}
