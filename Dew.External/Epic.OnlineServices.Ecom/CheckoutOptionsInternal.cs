using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CheckoutOptionsInternal : ISettable<CheckoutOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_OverrideCatalogNamespace;

	private uint m_EntryCount;

	private IntPtr m_Entries;

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

	public CheckoutEntry[] Entries
	{
		set
		{
			Helper.Set<CheckoutEntry, CheckoutEntryInternal>(ref value, ref m_Entries, out m_EntryCount);
		}
	}

	public void Set(ref CheckoutOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		OverrideCatalogNamespace = other.OverrideCatalogNamespace;
		Entries = other.Entries;
	}

	public void Set(ref CheckoutOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			OverrideCatalogNamespace = other.Value.OverrideCatalogNamespace;
			Entries = other.Value.Entries;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_OverrideCatalogNamespace);
		Helper.Dispose(ref m_Entries);
	}
}
