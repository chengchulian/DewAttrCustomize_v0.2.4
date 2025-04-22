using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AudioDevicesChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioDevicesChangedCallbackInfo>, ISettable<AudioDevicesChangedCallbackInfo>, IDisposable
{
	private IntPtr m_ClientData;

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

	public void Set(ref AudioDevicesChangedCallbackInfo other)
	{
		ClientData = other.ClientData;
	}

	public void Set(ref AudioDevicesChangedCallbackInfo? other)
	{
		if (other.HasValue)
		{
			ClientData = other.Value.ClientData;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientData);
	}

	public void Get(out AudioDevicesChangedCallbackInfo output)
	{
		output = default(AudioDevicesChangedCallbackInfo);
		output.Set(ref this);
	}
}
