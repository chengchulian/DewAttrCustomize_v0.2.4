using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddNotifyUserLoginStatusChangedOptionsInternal : ISettable<AddNotifyUserLoginStatusChangedOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref AddNotifyUserLoginStatusChangedOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref AddNotifyUserLoginStatusChangedOptions? other)
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
