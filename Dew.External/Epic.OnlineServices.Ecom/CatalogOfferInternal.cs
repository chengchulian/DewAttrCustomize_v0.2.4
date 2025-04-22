using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CatalogOfferInternal : IGettable<CatalogOffer>, ISettable<CatalogOffer>, IDisposable
{
	private int m_ApiVersion;

	private int m_ServerIndex;

	private IntPtr m_CatalogNamespace;

	private IntPtr m_Id;

	private IntPtr m_TitleText;

	private IntPtr m_DescriptionText;

	private IntPtr m_LongDescriptionText;

	private IntPtr m_TechnicalDetailsText_DEPRECATED;

	private IntPtr m_CurrencyCode;

	private Result m_PriceResult;

	private uint m_OriginalPrice_DEPRECATED;

	private uint m_CurrentPrice_DEPRECATED;

	private byte m_DiscountPercentage;

	private long m_ExpirationTimestamp;

	private uint m_PurchasedCount_DEPRECATED;

	private int m_PurchaseLimit;

	private int m_AvailableForPurchase;

	private ulong m_OriginalPrice64;

	private ulong m_CurrentPrice64;

	private uint m_DecimalPoint;

	private long m_ReleaseDateTimestamp;

	private long m_EffectiveDateTimestamp;

	public int ServerIndex
	{
		get
		{
			return m_ServerIndex;
		}
		set
		{
			m_ServerIndex = value;
		}
	}

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

	public Utf8String TechnicalDetailsText_DEPRECATED
	{
		get
		{
			Helper.Get(m_TechnicalDetailsText_DEPRECATED, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_TechnicalDetailsText_DEPRECATED);
		}
	}

	public Utf8String CurrencyCode
	{
		get
		{
			Helper.Get(m_CurrencyCode, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_CurrencyCode);
		}
	}

	public Result PriceResult
	{
		get
		{
			return m_PriceResult;
		}
		set
		{
			m_PriceResult = value;
		}
	}

	public uint OriginalPrice_DEPRECATED
	{
		get
		{
			return m_OriginalPrice_DEPRECATED;
		}
		set
		{
			m_OriginalPrice_DEPRECATED = value;
		}
	}

	public uint CurrentPrice_DEPRECATED
	{
		get
		{
			return m_CurrentPrice_DEPRECATED;
		}
		set
		{
			m_CurrentPrice_DEPRECATED = value;
		}
	}

	public byte DiscountPercentage
	{
		get
		{
			return m_DiscountPercentage;
		}
		set
		{
			m_DiscountPercentage = value;
		}
	}

	public long ExpirationTimestamp
	{
		get
		{
			return m_ExpirationTimestamp;
		}
		set
		{
			m_ExpirationTimestamp = value;
		}
	}

	public uint PurchasedCount_DEPRECATED
	{
		get
		{
			return m_PurchasedCount_DEPRECATED;
		}
		set
		{
			m_PurchasedCount_DEPRECATED = value;
		}
	}

	public int PurchaseLimit
	{
		get
		{
			return m_PurchaseLimit;
		}
		set
		{
			m_PurchaseLimit = value;
		}
	}

	public bool AvailableForPurchase
	{
		get
		{
			Helper.Get(m_AvailableForPurchase, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AvailableForPurchase);
		}
	}

	public ulong OriginalPrice64
	{
		get
		{
			return m_OriginalPrice64;
		}
		set
		{
			m_OriginalPrice64 = value;
		}
	}

	public ulong CurrentPrice64
	{
		get
		{
			return m_CurrentPrice64;
		}
		set
		{
			m_CurrentPrice64 = value;
		}
	}

	public uint DecimalPoint
	{
		get
		{
			return m_DecimalPoint;
		}
		set
		{
			m_DecimalPoint = value;
		}
	}

	public long ReleaseDateTimestamp
	{
		get
		{
			return m_ReleaseDateTimestamp;
		}
		set
		{
			m_ReleaseDateTimestamp = value;
		}
	}

	public long EffectiveDateTimestamp
	{
		get
		{
			return m_EffectiveDateTimestamp;
		}
		set
		{
			m_EffectiveDateTimestamp = value;
		}
	}

	public void Set(ref CatalogOffer other)
	{
		m_ApiVersion = 5;
		ServerIndex = other.ServerIndex;
		CatalogNamespace = other.CatalogNamespace;
		Id = other.Id;
		TitleText = other.TitleText;
		DescriptionText = other.DescriptionText;
		LongDescriptionText = other.LongDescriptionText;
		TechnicalDetailsText_DEPRECATED = other.TechnicalDetailsText_DEPRECATED;
		CurrencyCode = other.CurrencyCode;
		PriceResult = other.PriceResult;
		OriginalPrice_DEPRECATED = other.OriginalPrice_DEPRECATED;
		CurrentPrice_DEPRECATED = other.CurrentPrice_DEPRECATED;
		DiscountPercentage = other.DiscountPercentage;
		ExpirationTimestamp = other.ExpirationTimestamp;
		PurchasedCount_DEPRECATED = other.PurchasedCount_DEPRECATED;
		PurchaseLimit = other.PurchaseLimit;
		AvailableForPurchase = other.AvailableForPurchase;
		OriginalPrice64 = other.OriginalPrice64;
		CurrentPrice64 = other.CurrentPrice64;
		DecimalPoint = other.DecimalPoint;
		ReleaseDateTimestamp = other.ReleaseDateTimestamp;
		EffectiveDateTimestamp = other.EffectiveDateTimestamp;
	}

	public void Set(ref CatalogOffer? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 5;
			ServerIndex = other.Value.ServerIndex;
			CatalogNamespace = other.Value.CatalogNamespace;
			Id = other.Value.Id;
			TitleText = other.Value.TitleText;
			DescriptionText = other.Value.DescriptionText;
			LongDescriptionText = other.Value.LongDescriptionText;
			TechnicalDetailsText_DEPRECATED = other.Value.TechnicalDetailsText_DEPRECATED;
			CurrencyCode = other.Value.CurrencyCode;
			PriceResult = other.Value.PriceResult;
			OriginalPrice_DEPRECATED = other.Value.OriginalPrice_DEPRECATED;
			CurrentPrice_DEPRECATED = other.Value.CurrentPrice_DEPRECATED;
			DiscountPercentage = other.Value.DiscountPercentage;
			ExpirationTimestamp = other.Value.ExpirationTimestamp;
			PurchasedCount_DEPRECATED = other.Value.PurchasedCount_DEPRECATED;
			PurchaseLimit = other.Value.PurchaseLimit;
			AvailableForPurchase = other.Value.AvailableForPurchase;
			OriginalPrice64 = other.Value.OriginalPrice64;
			CurrentPrice64 = other.Value.CurrentPrice64;
			DecimalPoint = other.Value.DecimalPoint;
			ReleaseDateTimestamp = other.Value.ReleaseDateTimestamp;
			EffectiveDateTimestamp = other.Value.EffectiveDateTimestamp;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_CatalogNamespace);
		Helper.Dispose(ref m_Id);
		Helper.Dispose(ref m_TitleText);
		Helper.Dispose(ref m_DescriptionText);
		Helper.Dispose(ref m_LongDescriptionText);
		Helper.Dispose(ref m_TechnicalDetailsText_DEPRECATED);
		Helper.Dispose(ref m_CurrencyCode);
	}

	public void Get(out CatalogOffer output)
	{
		output = default(CatalogOffer);
		output.Set(ref this);
	}
}
