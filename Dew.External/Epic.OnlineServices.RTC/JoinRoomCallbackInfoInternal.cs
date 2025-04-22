using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct JoinRoomCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinRoomCallbackInfo>, ISettable<JoinRoomCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

	private uint m_RoomOptionsCount;

	private IntPtr m_RoomOptions;

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

	public Option[] RoomOptions
	{
		get
		{
			Helper.Get<OptionInternal, Option>(m_RoomOptions, out var value, m_RoomOptionsCount);
			return value;
		}
		set
		{
			Helper.Set<Option, OptionInternal>(ref value, ref m_RoomOptions, out m_RoomOptionsCount);
		}
	}

	public void Set(ref JoinRoomCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		RoomOptions = other.RoomOptions;
	}

	public void Set(ref JoinRoomCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
			RoomOptions = other.Value.RoomOptions;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
		Helper.Dispose(ref m_RoomOptions);
	}

	public void Get(out JoinRoomCallbackInfo output)
	{
		output = default(JoinRoomCallbackInfo);
		output.Set(ref this);
	}
}
