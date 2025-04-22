namespace Epic.OnlineServices.Sessions;

public struct LeaveSessionRequestedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String SessionName { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref LeaveSessionRequestedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		SessionName = other.SessionName;
	}
}
