using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetAudioInputDevicesCountOptionsInternal : ISettable<GetAudioInputDevicesCountOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref GetAudioInputDevicesCountOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref GetAudioInputDevicesCountOptions? other)
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
