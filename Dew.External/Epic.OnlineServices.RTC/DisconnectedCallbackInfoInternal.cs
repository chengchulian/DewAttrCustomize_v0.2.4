using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct DisconnectedCallbackInfoInternal : ICallbackInfoInternal, IGettable<DisconnectedCallbackInfo>, ISettable<DisconnectedCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_LocalUserId;

	private IntPtr m_RoomName;

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

	public void Set(ref DisconnectedCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
	}

	public void Set(ref DisconnectedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			LocalUserId = other.Value.LocalUserId;
			RoomName = other.Value.RoomName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RoomName);
	}

	public void Get(out DisconnectedCallbackInfo output)
	{
		output = default(DisconnectedCallbackInfo);
		output.Set(ref this);
	}
}
