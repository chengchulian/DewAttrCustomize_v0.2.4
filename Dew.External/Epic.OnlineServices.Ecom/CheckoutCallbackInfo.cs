namespace Epic.OnlineServices.Ecom;

public struct CheckoutCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public Utf8String TransactionId { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref CheckoutCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TransactionId = other.TransactionId;
	}
}
