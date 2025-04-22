using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryNATTypeOptionsInternal : ISettable<QueryNATTypeOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref QueryNATTypeOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref QueryNATTypeOptions? other)
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
