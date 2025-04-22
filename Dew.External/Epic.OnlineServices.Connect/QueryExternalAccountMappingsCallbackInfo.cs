namespace Epic.OnlineServices.Connect;

public struct QueryExternalAccountMappingsCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref QueryExternalAccountMappingsCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
	}
}
