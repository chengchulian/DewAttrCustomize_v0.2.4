namespace Epic.OnlineServices.Connect;

public struct QueryExternalAccountMappingsOptions
{
	public ProductUserId LocalUserId { get; set; }

	public ExternalAccountType AccountIdType { get; set; }

	public Utf8String[] ExternalAccountIds { get; set; }
}
