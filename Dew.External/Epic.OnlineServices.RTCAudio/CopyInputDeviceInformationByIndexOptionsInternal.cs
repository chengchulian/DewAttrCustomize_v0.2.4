using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyInputDeviceInformationByIndexOptionsInternal : ISettable<CopyInputDeviceInformationByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_DeviceIndex;

	public uint DeviceIndex
	{
		set
		{
			m_DeviceIndex = value;
		}
	}

	public void Set(ref CopyInputDeviceInformationByIndexOptions other)
	{
		m_ApiVersion = 1;
		DeviceIndex = other.DeviceIndex;
	}

	public void Set(ref CopyInputDeviceInformationByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			DeviceIndex = other.Value.DeviceIndex;
		}
	}

	public void Dispose()
	{
	}
}
