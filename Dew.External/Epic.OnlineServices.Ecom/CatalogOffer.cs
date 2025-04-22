namespace Epic.OnlineServices.Ecom;

public struct CatalogOffer
{
	public int ServerIndex { get; set; }

	public Utf8String CatalogNamespace { get; set; }

	public Utf8String Id { get; set; }

	public Utf8String TitleText { get; set; }

	public Utf8String DescriptionText { get; set; }

	public Utf8String LongDescriptionText { get; set; }

	public Utf8String TechnicalDetailsText_DEPRECATED { get; set; }

	public Utf8String CurrencyCode { get; set; }

	public Result PriceResult { get; set; }

	public uint OriginalPrice_DEPRECATED { get; set; }

	public uint CurrentPrice_DEPRECATED { get; set; }

	public byte DiscountPercentage { get; set; }

	public long ExpirationTimestamp { get; set; }

	public uint PurchasedCount_DEPRECATED { get; set; }

	public int PurchaseLimit { get; set; }

	public bool AvailableForPurchase { get; set; }

	public ulong OriginalPrice64 { get; set; }

	public ulong CurrentPrice64 { get; set; }

	public uint DecimalPoint { get; set; }

	public long ReleaseDateTimestamp { get; set; }

	public long EffectiveDateTimestamp { get; set; }

	internal void Set(ref CatalogOfferInternal other)
	{
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
}
