using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyMessageToClientOptionsInternal : ISettable<AddNotifyMessageToClientOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyMessageToClientOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyMessageToClientOptions? other)
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
