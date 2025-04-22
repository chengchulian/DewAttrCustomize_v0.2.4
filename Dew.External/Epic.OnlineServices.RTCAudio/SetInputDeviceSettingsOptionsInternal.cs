using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetInputDeviceSettingsOptionsInternal : ISettable<SetInputDeviceSettingsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RealDeviceId;

	private int m_PlatformAEC;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String RealDeviceId
	{
		set
		{
			Helper.Set(value, ref m_RealDeviceId);
		}
	}

	public bool PlatformAEC
	{
		set
		{
			Helper.Set(value, ref m_PlatformAEC);
		}
	}

	public void Set(ref SetInputDeviceSettingsOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RealDeviceId = other.RealDeviceId;
		PlatformAEC = other.PlatformAEC;
	}

	public void Set(ref SetInputDeviceSettingsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RealDeviceId = other.Value.RealDeviceId;
			PlatformAEC = other.Value.PlatformAEC;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RealDeviceId);
	}
}
