namespace Epic.OnlineServices.Ecom;

public struct CopyOfferItemByIndexOptions
{
	public EpicAccountId LocalUserId { get; set; }

	public Utf8String OfferId { get; set; }

	public uint ItemIndex { get; set; }
}
