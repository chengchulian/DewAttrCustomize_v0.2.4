using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct UnregisterPeerOptionsInternal : ISettable<UnregisterPeerOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PeerHandle;

	public IntPtr PeerHandle
	{
		set
		{
			m_PeerHandle = value;
		}
	}

	public void Set(ref UnregisterPeerOptions other)
	{
		m_ApiVersion = 1;
		PeerHandle = other.PeerHandle;
	}

	public void Set(ref UnregisterPeerOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PeerHandle = other.Value.PeerHandle;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PeerHandle);
	}
}
