namespace Epic.OnlineServices.RTCAudio;

public struct AudioInputStateCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }

	public RTCAudioInputStatus Status { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref AudioInputStateCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		Status = other.Status;
	}
}
