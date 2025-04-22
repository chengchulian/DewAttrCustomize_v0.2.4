namespace Epic.OnlineServices.Ecom;

public struct RedeemEntitlementsCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public uint RedeemedEntitlementIdsCount { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref RedeemEntitlementsCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RedeemedEntitlementIdsCount = other.RedeemedEntitlementIdsCount;
	}
}
