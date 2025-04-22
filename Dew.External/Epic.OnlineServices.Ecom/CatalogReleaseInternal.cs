using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CatalogReleaseInternal : IGettable<CatalogRelease>, ISettable<CatalogRelease>, IDisposable
{
	private int m_ApiVersion;

	private uint m_CompatibleAppIdCount;

	private IntPtr m_CompatibleAppIds;

	private uint m_CompatiblePlatformCount;

	private IntPtr m_CompatiblePlatforms;

	private IntPtr m_ReleaseNote;

	public Utf8String[] CompatibleAppIds
	{
		get
		{
			Helper.Get<Utf8String>(m_CompatibleAppIds, out var value, m_CompatibleAppIdCount, isArrayItemAllocated: true);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_CompatibleAppIds, isArrayItemAllocated: true, out m_CompatibleAppIdCount);
		}
	}

	public Utf8String[] CompatiblePlatforms
	{
		get
		{
			Helper.Get<Utf8String>(m_CompatiblePlatforms, out var value, m_CompatiblePlatformCount, isArrayItemAllocated: true);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_CompatiblePlatforms, isArrayItemAllocated: true, out m_CompatiblePlatformCount);
		}
	}

	public Utf8String ReleaseNote
	{
		get
		{
			Helper.Get(m_ReleaseNote, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ReleaseNote);
		}
	}

	public void Set(ref CatalogRelease other)
	{
		m_ApiVersion = 1;
		CompatibleAppIds = other.CompatibleAppIds;
		CompatiblePlatforms = other.CompatiblePlatforms;
		ReleaseNote = other.ReleaseNote;
	}

	public void Set(ref CatalogRelease? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			CompatibleAppIds = other.Value.CompatibleAppIds;
			CompatiblePlatforms = other.Value.CompatiblePlatforms;
			ReleaseNote = other.Value.ReleaseNote;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_CompatibleAppIds);
		Helper.Dispose(ref m_CompatiblePlatforms);
		Helper.Dispose(ref m_ReleaseNote);
	}

	public void Get(out CatalogRelease output)
	{
		output = default(CatalogRelease);
		output.Set(ref this);
	}
}
