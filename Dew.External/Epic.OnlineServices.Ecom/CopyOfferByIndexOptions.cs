namespace Epic.OnlineServices.Ecom;

public struct CopyOfferByIndexOptions
{
	public EpicAccountId LocalUserId { get; set; }

	public uint OfferIndex { get; set; }
}
