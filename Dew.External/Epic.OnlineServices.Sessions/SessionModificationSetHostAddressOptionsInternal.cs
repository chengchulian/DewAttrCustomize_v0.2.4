using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationSetHostAddressOptionsInternal : ISettable<SessionModificationSetHostAddressOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_HostAddress;

	public Utf8String HostAddress
	{
		set
		{
			Helper.Set(value, ref m_HostAddress);
		}
	}

	public void Set(ref SessionModificationSetHostAddressOptions other)
	{
		m_ApiVersion = 1;
		HostAddress = other.HostAddress;
	}

	public void Set(ref SessionModificationSetHostAddressOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			HostAddress = other.Value.HostAddress;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_HostAddress);
	}
}
