using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetAudioOutputSettingsOptionsInternal : ISettable<SetAudioOutputSettingsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_DeviceId;

	private float m_Volume;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String DeviceId
	{
		set
		{
			Helper.Set(value, ref m_DeviceId);
		}
	}

	public float Volume
	{
		set
		{
			m_Volume = value;
		}
	}

	public void Set(ref SetAudioOutputSettingsOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		DeviceId = other.DeviceId;
		Volume = other.Volume;
	}

	public void Set(ref SetAudioOutputSettingsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			DeviceId = other.Value.DeviceId;
			Volume = other.Value.Volume;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_DeviceId);
	}
}
