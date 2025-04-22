using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyCustomInviteRejectedOptionsInternal : ISettable<AddNotifyCustomInviteRejectedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyCustomInviteRejectedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyCustomInviteRejectedOptions? other)
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
