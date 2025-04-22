using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnUnregisterPlatformUserCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnUnregisterPlatformUserCallbackInfo>, ISettable<OnUnregisterPlatformUserCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_PlatformUserId;

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

	public Utf8String PlatformUserId
	{
		get
		{
			Helper.Get(m_PlatformUserId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_PlatformUserId);
		}
	}

	public void Set(ref OnUnregisterPlatformUserCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		PlatformUserId = other.PlatformUserId;
	}

	public void Set(ref OnUnregisterPlatformUserCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			PlatformUserId = other.Value.PlatformUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_PlatformUserId);
	}

	public void Get(out OnUnregisterPlatformUserCallbackInfo output)
	{
		output = default(OnUnregisterPlatformUserCallbackInfo);
		output.Set(ref this);
	}
}
