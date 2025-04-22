using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyAudioDevicesChangedOptionsInternal : ISettable<AddNotifyAudioDevicesChangedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyAudioDevicesChangedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyAudioDevicesChangedOptions? other)
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
