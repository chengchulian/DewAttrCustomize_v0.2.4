using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UpdateSessionCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateSessionCallbackInfo>, ISettable<UpdateSessionCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_SessionName;

	private IntPtr m_SessionId;

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

	public Utf8String SessionName
	{
		get
		{
			Helper.Get(m_SessionName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SessionName);
		}
	}

	public Utf8String SessionId
	{
		get
		{
			Helper.Get(m_SessionId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SessionId);
		}
	}

	public void Set(ref UpdateSessionCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		SessionName = other.SessionName;
		SessionId = other.SessionId;
	}

	public void Set(ref UpdateSessionCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			SessionName = other.Value.SessionName;
			SessionId = other.Value.SessionId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_SessionName);
		Helper.Dispose(ref m_SessionId);
	}

	public void Get(out UpdateSessionCallbackInfo output)
	{
		output = default(UpdateSessionCallbackInfo);
		output.Set(ref this);
	}
}
