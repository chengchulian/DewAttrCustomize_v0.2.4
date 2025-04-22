using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyCustomInviteReceivedOptionsInternal : ISettable<AddNotifyCustomInviteReceivedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyCustomInviteReceivedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyCustomInviteReceivedOptions? other)
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
