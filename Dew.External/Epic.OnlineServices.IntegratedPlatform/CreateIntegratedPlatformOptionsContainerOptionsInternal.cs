using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CreateIntegratedPlatformOptionsContainerOptionsInternal : ISettable<CreateIntegratedPlatformOptionsContainerOptions>, IDisposable
{
	private int m_ApiVersion;

	public void Set(ref CreateIntegratedPlatformOptionsContainerOptions other)
	{
		m_ApiVersion = 1;
	}

	public void Set(ref CreateIntegratedPlatformOptionsContainerOptions? other)
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
