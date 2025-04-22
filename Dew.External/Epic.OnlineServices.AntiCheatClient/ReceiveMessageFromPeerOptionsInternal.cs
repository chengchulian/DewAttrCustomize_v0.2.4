using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ReceiveMessageFromPeerOptionsInternal : ISettable<ReceiveMessageFromPeerOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PeerHandle;

	private uint m_DataLengthBytes;

	private IntPtr m_Data;

	public IntPtr PeerHandle
	{
		set
		{
			m_PeerHandle = value;
		}
	}

	public ArraySegment<byte> Data
	{
		set
		{
			Helper.Set(value, ref m_Data, out m_DataLengthBytes);
		}
	}

	public void Set(ref ReceiveMessageFromPeerOptions other)
	{
		m_ApiVersion = 1;
		PeerHandle = other.PeerHandle;
		Data = other.Data;
	}

	public void Set(ref ReceiveMessageFromPeerOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PeerHandle = other.Value.PeerHandle;
			Data = other.Value.Data;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PeerHandle);
		Helper.Dispose(ref m_Data);
	}
}
