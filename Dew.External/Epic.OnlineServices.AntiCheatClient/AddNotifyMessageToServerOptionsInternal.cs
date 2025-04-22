using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyMessageToServerOptionsInternal : ISettable<AddNotifyMessageToServerOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyMessageToServerOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyMessageToServerOptions? other)
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
