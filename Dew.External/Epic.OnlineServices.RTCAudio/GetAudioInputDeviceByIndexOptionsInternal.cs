using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetAudioInputDeviceByIndexOptionsInternal : ISettable<GetAudioInputDeviceByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_DeviceInfoIndex;

	public uint DeviceInfoIndex
	{
		set
		{
			m_DeviceInfoIndex = value;
		}
	}

	public void Set(ref GetAudioInputDeviceByIndexOptions other)
	{
		m_ApiVersion = 1;
		DeviceInfoIndex = other.DeviceInfoIndex;
	}

	public void Set(ref GetAudioInputDeviceByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			DeviceInfoIndex = other.Value.DeviceInfoIndex;
		}
	}

	public void Dispose()
	{
	}
}
