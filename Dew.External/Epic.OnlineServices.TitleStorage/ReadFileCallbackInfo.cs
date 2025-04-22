namespace Epic.OnlineServices.TitleStorage;

public struct ReadFileCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String Filename { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref ReadFileCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		Filename = other.Filename;
	}
}
