using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyJoinGameAcceptedOptionsInternal : ISettable<AddNotifyJoinGameAcceptedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyJoinGameAcceptedOptions other)
	{
		m_ApiVersion = 2;
	}

	public void Set(ref AddNotifyJoinGameAcceptedOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
		}
	}

	public void Dispose()
	{
	}
}
