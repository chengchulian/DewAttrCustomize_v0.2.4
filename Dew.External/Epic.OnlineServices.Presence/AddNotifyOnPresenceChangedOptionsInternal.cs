using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyOnPresenceChangedOptionsInternal : ISettable<AddNotifyOnPresenceChangedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyOnPresenceChangedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyOnPresenceChangedOptions? other)
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
