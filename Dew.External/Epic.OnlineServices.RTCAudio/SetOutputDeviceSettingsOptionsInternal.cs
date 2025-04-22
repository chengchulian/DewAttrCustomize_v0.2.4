using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetOutputDeviceSettingsOptionsInternal : ISettable<SetOutputDeviceSettingsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_RealDeviceId;

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

	public void Set(ref SetOutputDeviceSettingsOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RealDeviceId = other.RealDeviceId;
	}

	public void Set(ref SetOutputDeviceSettingsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RealDeviceId = other.Value.RealDeviceId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_RealDeviceId);
	}
}
