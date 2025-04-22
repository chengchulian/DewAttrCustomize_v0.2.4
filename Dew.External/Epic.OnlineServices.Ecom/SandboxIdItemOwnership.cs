namespace Epic.OnlineServices.Ecom;

public struct SandboxIdItemOwnership
{
	public Utf8String SandboxId { get; set; }

	public Utf8String[] OwnedCatalogItemIds { get; set; }

	internal void Set(ref SandboxIdItemOwnershipInternal other)
	{
		SandboxId = other.SandboxId;
		OwnedCatalogItemIds = other.OwnedCatalogItemIds;
	}
}
