namespace Epic.OnlineServices.Ecom;

public struct ItemOwnership
{
	public Utf8String Id { get; set; }

	public OwnershipStatus OwnershipStatus { get; set; }

	internal void Set(ref ItemOwnershipInternal other)
	{
		Id = other.Id;
		OwnershipStatus = other.OwnershipStatus;
	}
}
