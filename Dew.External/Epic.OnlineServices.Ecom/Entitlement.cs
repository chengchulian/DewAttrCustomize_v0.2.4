namespace Epic.OnlineServices.Ecom;

public struct Entitlement
{
	public Utf8String EntitlementName { get; set; }

	public Utf8String EntitlementId { get; set; }

	public Utf8String CatalogItemId { get; set; }

	public int ServerIndex { get; set; }

	public bool Redeemed { get; set; }

	public long EndTimestamp { get; set; }

	internal void Set(ref EntitlementInternal other)
	{
		EntitlementName = other.EntitlementName;
		EntitlementId = other.EntitlementId;
		CatalogItemId = other.CatalogItemId;
		ServerIndex = other.ServerIndex;
		Redeemed = other.Redeemed;
		EndTimestamp = other.EndTimestamp;
	}
}
