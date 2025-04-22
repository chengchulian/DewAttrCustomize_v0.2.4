using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CreateDeviceIdOptionsInternal : ISettable<CreateDeviceIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_DeviceModel;

	public Utf8String DeviceModel
	{
		set
		{
			Helper.Set(value, ref m_DeviceModel);
		}
	}

	public void Set(ref CreateDeviceIdOptions other)
	{
		m_ApiVersion = 1;
		DeviceModel = other.DeviceModel;
	}

	public void Set(ref CreateDeviceIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			DeviceModel = other.Value.DeviceModel;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_DeviceModel);
	}
}
