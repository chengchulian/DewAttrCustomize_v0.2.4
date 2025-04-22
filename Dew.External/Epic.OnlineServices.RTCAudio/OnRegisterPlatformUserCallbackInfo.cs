namespace Epic.OnlineServices.RTCAudio;

public struct OnRegisterPlatformUserCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public Utf8String PlatformUserId { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref OnRegisterPlatformUserCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		PlatformUserId = other.PlatformUserId;
	}
}
