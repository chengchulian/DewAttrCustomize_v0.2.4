namespace Epic.OnlineServices.Ecom;

public struct CheckoutEntry
{
	public Utf8String OfferId { get; set; }

	internal void Set(ref CheckoutEntryInternal other)
	{
		OfferId = other.OfferId;
	}
}
