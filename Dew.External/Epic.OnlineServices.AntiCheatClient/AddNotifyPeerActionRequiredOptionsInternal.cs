using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyPeerActionRequiredOptionsInternal : ISettable<AddNotifyPeerActionRequiredOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyPeerActionRequiredOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyPeerActionRequiredOptions? other)
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
