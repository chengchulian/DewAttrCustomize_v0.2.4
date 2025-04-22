namespace Epic.OnlineServices.RTCAdmin;

public struct QueryJoinRoomTokenCompleteCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public Utf8String RoomName { get; set; }

	public Utf8String ClientBaseUrl { get; set; }

	public uint QueryId { get; set; }

	public uint TokenCount { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref QueryJoinRoomTokenCompleteCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		RoomName = other.RoomName;
		ClientBaseUrl = other.ClientBaseUrl;
		QueryId = other.QueryId;
		TokenCount = other.TokenCount;
	}
}
