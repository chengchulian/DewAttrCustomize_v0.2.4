namespace Epic.OnlineServices.Ecom;

public struct CatalogItem
{
	public Utf8String CatalogNamespace { get; set; }

	public Utf8String Id { get; set; }

	public Utf8String EntitlementName { get; set; }

	public Utf8String TitleText { get; set; }

	public Utf8String DescriptionText { get; set; }

	public Utf8String LongDescriptionText { get; set; }

	public Utf8String TechnicalDetailsText { get; set; }

	public Utf8String DeveloperText { get; set; }

	public EcomItemType ItemType { get; set; }

	public long EntitlementEndTimestamp { get; set; }

	internal void Set(ref CatalogItemInternal other)
	{
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
}
