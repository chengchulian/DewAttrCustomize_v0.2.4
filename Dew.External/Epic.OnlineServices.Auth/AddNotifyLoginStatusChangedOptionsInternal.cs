using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyLoginStatusChangedOptionsInternal : ISettable<AddNotifyLoginStatusChangedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyLoginStatusChangedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyLoginStatusChangedOptions? other)
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
