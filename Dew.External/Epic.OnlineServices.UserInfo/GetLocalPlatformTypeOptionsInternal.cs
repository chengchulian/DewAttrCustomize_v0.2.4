using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetLocalPlatformTypeOptionsInternal : ISettable<GetLocalPlatformTypeOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref GetLocalPlatformTypeOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref GetLocalPlatformTypeOptions? other)
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
