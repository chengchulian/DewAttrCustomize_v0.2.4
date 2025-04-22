using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyClientIntegrityViolatedOptionsInternal : ISettable<AddNotifyClientIntegrityViolatedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyClientIntegrityViolatedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyClientIntegrityViolatedOptions? other)
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
