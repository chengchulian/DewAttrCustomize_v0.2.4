namespace Epic.OnlineServices.RTC;

public struct DisconnectedCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref DisconnectedCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
	}
}
