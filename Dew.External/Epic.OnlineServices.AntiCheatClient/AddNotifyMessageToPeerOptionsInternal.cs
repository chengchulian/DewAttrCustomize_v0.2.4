using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyMessageToPeerOptionsInternal : ISettable<AddNotifyMessageToPeerOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyMessageToPeerOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyMessageToPeerOptions? other)
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
