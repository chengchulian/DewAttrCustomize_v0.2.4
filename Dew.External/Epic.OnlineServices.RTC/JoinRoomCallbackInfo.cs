namespace Epic.OnlineServices.RTC;

public struct JoinRoomCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }

	public Option[] RoomOptions { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref JoinRoomCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		RoomOptions = other.RoomOptions;
	}
}
