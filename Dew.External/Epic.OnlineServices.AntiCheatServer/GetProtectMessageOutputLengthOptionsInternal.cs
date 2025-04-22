using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetProtectMessageOutputLengthOptionsInternal : ISettable<GetProtectMessageOutputLengthOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_DataLengthBytes;

	public uint DataLengthBytes
	{
		set
		{
			m_DataLengthBytes = value;
		}
	}

	public void Set(ref GetProtectMessageOutputLengthOptions other)
	{
		m_ApiVersion = 1;
		DataLengthBytes = other.DataLengthBytes;
	}

	public void Set(ref GetProtectMessageOutputLengthOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			DataLengthBytes = other.Value.DataLengthBytes;
		}
	}

	public void Dispose()
	{
	}
}
