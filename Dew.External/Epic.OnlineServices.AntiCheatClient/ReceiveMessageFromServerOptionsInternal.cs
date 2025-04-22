using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ReceiveMessageFromServerOptionsInternal : ISettable<ReceiveMessageFromServerOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_DataLengthBytes;

	private IntPtr m_Data;

	public ArraySegment<byte> Data
	{
		set
		{
			Helper.Set(value, ref m_Data, out m_DataLengthBytes);
		}
	}

	public void Set(ref ReceiveMessageFromServerOptions other)
	{
		m_ApiVersion = 1;
		Data = other.Data;
	}

	public void Set(ref ReceiveMessageFromServerOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Data = other.Value.Data;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Data);
	}
}
