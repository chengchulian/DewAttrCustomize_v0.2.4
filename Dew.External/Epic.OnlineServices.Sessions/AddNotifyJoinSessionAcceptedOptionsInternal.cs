using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyJoinSessionAcceptedOptionsInternal : ISettable<AddNotifyJoinSessionAcceptedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyJoinSessionAcceptedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyJoinSessionAcceptedOptions? other)
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
