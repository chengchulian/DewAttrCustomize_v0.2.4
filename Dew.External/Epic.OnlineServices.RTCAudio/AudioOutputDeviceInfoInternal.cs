using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AudioOutputDeviceInfoInternal : IGettable<AudioOutputDeviceInfo>, ISettable<AudioOutputDeviceInfo>, IDisposable
{
	private int m_ApiVersion;

	private int m_DefaultDevice;

	private IntPtr m_DeviceId;

	private IntPtr m_DeviceName;

	public bool DefaultDevice
	{
		get
		{
			Helper.Get(m_DefaultDevice, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DefaultDevice);
		}
	}

	public Utf8String DeviceId
	{
		get
		{
			Helper.Get(m_DeviceId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DeviceId);
		}
	}

	public Utf8String DeviceName
	{
		get
		{
			Helper.Get(m_DeviceName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DeviceName);
		}
	}

	public void Set(ref AudioOutputDeviceInfo other)
	{
		m_ApiVersion = 1;
		DefaultDevice = other.DefaultDevice;
		DeviceId = other.DeviceId;
		DeviceName = other.DeviceName;
	}

	public void Set(ref AudioOutputDeviceInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			DefaultDevice = other.Value.DefaultDevice;
			DeviceId = other.Value.DeviceId;
			DeviceName = other.Value.DeviceName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_DeviceId);
		Helper.Dispose(ref m_DeviceName);
	}

	public void Get(out AudioOutputDeviceInfo output)
	{
		output = default(AudioOutputDeviceInfo);
		output.Set(ref this);
	}
}
