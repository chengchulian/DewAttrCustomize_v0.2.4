using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyRequestToJoinRejectedOptionsInternal : ISettable<AddNotifyRequestToJoinRejectedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyRequestToJoinRejectedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyRequestToJoinRejectedOptions? other)
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
