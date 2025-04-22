using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetDisplayPreferenceOptionsInternal : ISettable<SetDisplayPreferenceOptions>, IDisposable
{
	private int m_ApiVersion;

	private NotificationLocation m_NotificationLocation;

	public NotificationLocation NotificationLocation
	{
		set
		{
			m_NotificationLocation = value;
		}
	}

	public void Set(ref SetDisplayPreferenceOptions other)
	{
		m_ApiVersion = 1;
		NotificationLocation = other.NotificationLocation;
	}

	public void Set(ref SetDisplayPreferenceOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			NotificationLocation = other.Value.NotificationLocation;
		}
	}

	public void Dispose()
	{
	}
}
