using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetPortRangeOptionsInternal : ISettable<GetPortRangeOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref GetPortRangeOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref GetPortRangeOptions? other)
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
