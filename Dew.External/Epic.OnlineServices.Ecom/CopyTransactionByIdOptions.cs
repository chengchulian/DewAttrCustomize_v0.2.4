namespace Epic.OnlineServices.Ecom;

public struct CopyTransactionByIdOptions
{
	public EpicAccountId LocalUserId { get; set; }

	public Utf8String TransactionId { get; set; }
}
