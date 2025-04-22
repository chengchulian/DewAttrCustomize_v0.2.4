using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RTCRoomConnectionChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RTCRoomConnectionChangedCallbackInfo>, ISettable<RTCRoomConnectionChangedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

	private IntPtr m_LobbyId;

	private IntPtr m_LocalUserId;

	private int m_IsConnected;

	private Result m_DisconnectReason;

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

	public ProductUserId LocalUserId
	{
		get
		{
			Helper.Get(m_LocalUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public bool IsConnected
	{
		get
		{
			Helper.Get(m_IsConnected, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IsConnected);
		}
	}

	public Result DisconnectReason
	{
		get
		{
			return m_DisconnectReason;
		}
		set
		{
			m_DisconnectReason = value;
		}
	}

	public void Set(ref RTCRoomConnectionChangedCallbackInfo other)
	{
		ClientData = other.ClientData;
		LobbyId = other.LobbyId;
		LocalUserId = other.LocalUserId;
		IsConnected = other.IsConnected;
		DisconnectReason = other.DisconnectReason;
	}

	public void Set(ref RTCRoomConnectionChangedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
			LobbyId = other.Value.LobbyId;
			LocalUserId = other.Value.LocalUserId;
			IsConnected = other.Value.IsConnected;
			DisconnectReason = other.Value.DisconnectReason;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LobbyId);
		Helper.Dispose(ref m_LocalUserId);
	}

	public void Get(out RTCRoomConnectionChangedCallbackInfo output)
	{
		output = default(RTCRoomConnectionChangedCallbackInfo);
		output.Set(ref this);
	}
}
