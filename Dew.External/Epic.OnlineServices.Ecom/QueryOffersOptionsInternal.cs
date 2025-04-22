using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryOffersOptionsInternal : ISettable<QueryOffersOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_OverrideCatalogNamespace;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String OverrideCatalogNamespace
	{
		set
		{
			Helper.Set(value, ref m_OverrideCatalogNamespace);
		}
	}

	public void Set(ref QueryOffersOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		OverrideCatalogNamespace = other.OverrideCatalogNamespace;
	}

	public void Set(ref QueryOffersOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			OverrideCatalogNamespace = other.Value.OverrideCatalogNamespace;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_OverrideCatalogNamespace);
	}
}
