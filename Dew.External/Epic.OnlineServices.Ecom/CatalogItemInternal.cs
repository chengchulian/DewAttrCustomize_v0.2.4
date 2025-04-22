using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CatalogItemInternal : IGettable<CatalogItem>, ISettable<CatalogItem>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_CatalogNamespace;

	private IntPtr m_Id;

	private IntPtr m_EntitlementName;

	private IntPtr m_TitleText;

	private IntPtr m_DescriptionText;

	private IntPtr m_LongDescriptionText;

	private IntPtr m_TechnicalDetailsText;

	private IntPtr m_DeveloperText;

	private EcomItemType m_ItemType;

	private long m_EntitlementEndTimestamp;

	public Utf8String CatalogNamespace
	{
		get
		{
			Helper.Get(m_CatalogNamespace, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_CatalogNamespace);
		}
	}

	public Utf8String Id
	{
		get
		{
			Helper.Get(m_Id, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Id);
		}
	}

	public Utf8String EntitlementName
	{
		get
		{
			Helper.Get(m_EntitlementName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_EntitlementName);
		}
	}

	public Utf8String TitleText
	{
		get
		{
			Helper.Get(m_TitleText, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TitleText);
		}
	}

	public Utf8String DescriptionText
	{
		get
		{
			Helper.Get(m_DescriptionText, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DescriptionText);
		}
	}

	public Utf8String LongDescriptionText
	{
		get
		{
			Helper.Get(m_LongDescriptionText, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LongDescriptionText);
		}
	}

	public Utf8String TechnicalDetailsText
	{
		get
		{
			Helper.Get(m_TechnicalDetailsText, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TechnicalDetailsText);
		}
	}

	public Utf8String DeveloperText
	{
		get
		{
			Helper.Get(m_DeveloperText, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DeveloperText);
		}
	}

	public EcomItemType ItemType
	{
		get
		{
			return m_ItemType;
		}
		set
		{
			m_ItemType = value;
		}
	}

	public long EntitlementEndTimestamp
	{
		get
		{
			return m_EntitlementEndTimestamp;
		}
		set
		{
			m_EntitlementEndTimestamp = value;
		}
	}

	public void Set(ref CatalogItem other)
	{
		m_ApiVersion = 1;
		CatalogNamespace = other.CatalogNamespace;
		Id = other.Id;
		EntitlementName = other.EntitlementName;
		TitleText = other.TitleText;
		DescriptionText = other.DescriptionText;
		LongDescriptionText = other.LongDescriptionText;
		TechnicalDetailsText = other.TechnicalDetailsText;
		DeveloperText = other.DeveloperText;
		ItemType = other.ItemType;
		EntitlementEndTimestamp = other.EntitlementEndTimestamp;
	}

	public void Set(ref CatalogItem? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			CatalogNamespace = other.Value.CatalogNamespace;
			Id = other.Value.Id;
			EntitlementName = other.Value.EntitlementName;
			TitleText = other.Value.TitleText;
			DescriptionText = other.Value.DescriptionText;
			LongDescriptionText = other.Value.LongDescriptionText;
			TechnicalDetailsText = other.Value.TechnicalDetailsText;
			DeveloperText = other.Value.DeveloperText;
			ItemType = other.Value.ItemType;
			EntitlementEndTimestamp = other.Value.EntitlementEndTimestamp;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_CatalogNamespace);
		Helper.Dispose(ref m_Id);
		Helper.Dispose(ref m_EntitlementName);
		Helper.Dispose(ref m_TitleText);
		Helper.Dispose(ref m_DescriptionText);
		Helper.Dispose(ref m_LongDescriptionText);
		Helper.Dispose(ref m_TechnicalDetailsText);
		Helper.Dispose(ref m_DeveloperText);
	}

	public void Get(out CatalogItem output)
	{
		output = default(CatalogItem);
		output.Set(ref this);
	}
}
