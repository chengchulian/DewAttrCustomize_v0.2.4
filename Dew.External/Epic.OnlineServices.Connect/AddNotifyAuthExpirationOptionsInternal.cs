using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyAuthExpirationOptionsInternal : ISettable<AddNotifyAuthExpirationOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyAuthExpirationOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyAuthExpirationOptions? other)
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
