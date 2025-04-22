using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct KickCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<KickCompleteCallbackInfo>, ISettable<KickCompleteCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

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

	public void Set(ref KickCompleteCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
	}

	public void Set(ref KickCompleteCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
	}

	public void Get(out KickCompleteCallbackInfo output)
	{
		output = default(KickCompleteCallbackInfo);
		output.Set(ref this);
	}
}
