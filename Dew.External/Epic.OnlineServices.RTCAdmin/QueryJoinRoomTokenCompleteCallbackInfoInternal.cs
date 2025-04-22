using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryJoinRoomTokenCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryJoinRoomTokenCompleteCallbackInfo>, ISettable<QueryJoinRoomTokenCompleteCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_RoomName;

	private IntPtr m_ClientBaseUrl;

	private uint m_QueryId;

	private uint m_TokenCount;

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

	public Utf8String RoomName
	{
		get
		{
			Helper.Get(m_RoomName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RoomName);
		}
	}

	public Utf8String ClientBaseUrl
	{
		get
		{
			Helper.Get(m_ClientBaseUrl, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientBaseUrl);
		}
	}

	public uint QueryId
	{
		get
		{
			return m_QueryId;
		}
		set
		{
			m_QueryId = value;
		}
	}

	public uint TokenCount
	{
		get
		{
			return m_TokenCount;
		}
		set
		{
			m_TokenCount = value;
		}
	}

	public void Set(ref QueryJoinRoomTokenCompleteCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		RoomName = other.RoomName;
		ClientBaseUrl = other.ClientBaseUrl;
		QueryId = other.QueryId;
		TokenCount = other.TokenCount;
	}

	public void Set(ref QueryJoinRoomTokenCompleteCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			RoomName = other.Value.RoomName;
			ClientBaseUrl = other.Value.ClientBaseUrl;
			QueryId = other.Value.QueryId;
			TokenCount = other.Value.TokenCount;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_ClientBaseUrl);
	}

	public void Get(out QueryJoinRoomTokenCompleteCallbackInfo output)
	{
		output = default(QueryJoinRoomTokenCompleteCallbackInfo);
		output.Set(ref this);
	}
}
