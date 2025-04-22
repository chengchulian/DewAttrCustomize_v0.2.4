namespace Epic.OnlineServices.Ecom;

public struct CopyItemImageInfoByIndexOptions
{
	public EpicAccountId LocalUserId { get; set; }

	public Utf8String ItemId { get; set; }

	public uint ImageInfoIndex { get; set; }
}
