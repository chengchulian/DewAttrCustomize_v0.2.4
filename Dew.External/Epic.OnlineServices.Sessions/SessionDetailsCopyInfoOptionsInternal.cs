using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionDetailsCopyInfoOptionsInternal : ISettable<SessionDetailsCopyInfoOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref SessionDetailsCopyInfoOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref SessionDetailsCopyInfoOptions? other)
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
