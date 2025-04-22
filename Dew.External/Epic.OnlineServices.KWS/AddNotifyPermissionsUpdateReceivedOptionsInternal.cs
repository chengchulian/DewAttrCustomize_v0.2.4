using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyPermissionsUpdateReceivedOptionsInternal : ISettable<AddNotifyPermissionsUpdateReceivedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyPermissionsUpdateReceivedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyPermissionsUpdateReceivedOptions? other)
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
