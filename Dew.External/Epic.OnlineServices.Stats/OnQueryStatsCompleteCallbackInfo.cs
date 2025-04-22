namespace Epic.OnlineServices.Stats;

public struct OnQueryStatsCompleteCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref OnQueryStatsCompleteCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
	}
}
