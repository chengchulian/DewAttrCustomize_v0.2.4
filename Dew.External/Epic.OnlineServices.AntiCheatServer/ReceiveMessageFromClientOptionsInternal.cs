using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ReceiveMessageFromClientOptionsInternal : ISettable<ReceiveMessageFromClientOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ClientHandle;

	private uint m_DataLengthBytes;

	private IntPtr m_Data;

	public IntPtr ClientHandle
	{
		set
		{
			m_ClientHandle = value;
		}
	}

	public ArraySegment<byte> Data
	{
		set
		{
			Helper.Set(value, ref m_Data, out m_DataLengthBytes);
		}
	}

	public void Set(ref ReceiveMessageFromClientOptions other)
	{
		m_ApiVersion = 1;
		ClientHandle = other.ClientHandle;
		Data = other.Data;
	}

	public void Set(ref ReceiveMessageFromClientOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ClientHandle = other.Value.ClientHandle;
			Data = other.Value.Data;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientHandle);
		Helper.Dispose(ref m_Data);
	}
}
