using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryInputDevicesInformationOptionsInternal : ISettable<QueryInputDevicesInformationOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref QueryInputDevicesInformationOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref QueryInputDevicesInformationOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
		}
	}

	public void Dispose()
	{
	}
}
