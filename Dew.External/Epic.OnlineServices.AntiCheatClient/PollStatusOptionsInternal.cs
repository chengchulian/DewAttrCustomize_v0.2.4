using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PollStatusOptionsInternal : ISettable<PollStatusOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_OutMessageLength;

	public uint OutMessageLength
	{
		set
		{
			m_OutMessageLength = value;
		}
	}

	public void Set(ref PollStatusOptions other)
	{
		m_ApiVersion = 1;
		OutMessageLength = other.OutMessageLength;
	}

	public void Set(ref PollStatusOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			OutMessageLength = other.Value.OutMessageLength;
		}
	}

	public void Dispose()
	{
	}
}
