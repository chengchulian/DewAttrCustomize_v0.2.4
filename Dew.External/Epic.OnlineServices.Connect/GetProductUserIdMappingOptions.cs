namespace Epic.OnlineServices.Connect;

public struct GetProductUserIdMappingOptions
{
	public ProductUserId LocalUserId { get; set; }

	public ExternalAccountType AccountIdType { get; set; }

	public ProductUserId TargetProductUserId { get; set; }
}
