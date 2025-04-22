using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UnregisterClientOptionsInternal : ISettable<UnregisterClientOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ClientHandle;

	public IntPtr ClientHandle
	{
		set
		{
			m_ClientHandle = value;
		}
	}

	public void Set(ref UnregisterClientOptions other)
	{
		m_ApiVersion = 1;
		ClientHandle = other.ClientHandle;
	}

	public void Set(ref UnregisterClientOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ClientHandle = other.Value.ClientHandle;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientHandle);
	}
}
