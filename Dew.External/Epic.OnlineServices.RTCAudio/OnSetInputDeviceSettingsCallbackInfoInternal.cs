using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct OnSetInputDeviceSettingsCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnSetInputDeviceSettingsCallbackInfo>, ISettable<OnSetInputDeviceSettingsCallbackInfo>, IDisposable
{
	private Result m_ResultCode;

	private IntPtr m_ClientData;

	private IntPtr m_RealDeviceId;

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

	public Utf8String RealDeviceId
	{
		get
		{
			Helper.Get(m_RealDeviceId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RealDeviceId);
		}
	}

	public void Set(ref OnSetInputDeviceSettingsCallbackInfo other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		RealDeviceId = other.RealDeviceId;
	}

	public void Set(ref OnSetInputDeviceSettingsCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ResultCode = other.Value.ResultCode;
			ClientData = other.Value.ClientData;
			RealDeviceId = other.Value.RealDeviceId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
		Helper.Dispose(ref m_RealDeviceId);
	}

	public void Get(out OnSetInputDeviceSettingsCallbackInfo output)
	{
		output = default(OnSetInputDeviceSettingsCallbackInfo);
		output.Set(ref this);
	}
}
