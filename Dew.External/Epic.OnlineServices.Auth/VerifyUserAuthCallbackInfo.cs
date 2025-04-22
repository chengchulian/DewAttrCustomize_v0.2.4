namespace Epic.OnlineServices.Auth;

public struct VerifyUserAuthCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref VerifyUserAuthCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
	}
}
