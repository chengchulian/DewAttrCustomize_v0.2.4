namespace Epic.OnlineServices.Sanctions;

public struct QueryActivePlayerSanctionsCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref QueryActivePlayerSanctionsCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		TargetUserId = other.TargetUserId;
		LocalUserId = other.LocalUserId;
	}
}
