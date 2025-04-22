namespace Epic.OnlineServices.Ecom;

public struct CopyEntitlementByNameAndIndexOptions
{
	public EpicAccountId LocalUserId { get; set; }

	public Utf8String EntitlementName { get; set; }

	public uint Index { get; set; }
}
