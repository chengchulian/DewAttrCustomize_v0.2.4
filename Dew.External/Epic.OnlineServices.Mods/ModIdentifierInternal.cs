using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ModIdentifierInternal : IGettable<ModIdentifier>, ISettable<ModIdentifier>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_NamespaceId;

	private IntPtr m_ItemId;

	private IntPtr m_ArtifactId;

	private IntPtr m_Title;

	private IntPtr m_Version;

	public Utf8String NamespaceId
	{
		get
		{
			Helper.Get(m_NamespaceId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_NamespaceId);
		}
	}

	public Utf8String ItemId
	{
		get
		{
			Helper.Get(m_ItemId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ItemId);
		}
	}

	public Utf8String ArtifactId
	{
		get
		{
			Helper.Get(m_ArtifactId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ArtifactId);
		}
	}

	public Utf8String Title
	{
		get
		{
			Helper.Get(m_Title, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Title);
		}
	}

	public Utf8String Version
	{
		get
		{
			Helper.Get(m_Version, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Version);
		}
	}

	public void Set(ref ModIdentifier other)
	{
		m_ApiVersion = 1;
		NamespaceId = other.NamespaceId;
		ItemId = other.ItemId;
		ArtifactId = other.ArtifactId;
		Title = other.Title;
		Version = other.Version;
	}

	public void Set(ref ModIdentifier? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			NamespaceId = other.Value.NamespaceId;
			ItemId = other.Value.ItemId;
			ArtifactId = other.Value.ArtifactId;
			Title = other.Value.Title;
			Version = other.Value.Version;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_NamespaceId);
		Helper.Dispose(ref m_ItemId);
		Helper.Dispose(ref m_ArtifactId);
		Helper.Dispose(ref m_Title);
		Helper.Dispose(ref m_Version);
	}

	public void Get(out ModIdentifier output)
	{
		output = default(ModIdentifier);
		output.Set(ref this);
	}
}
