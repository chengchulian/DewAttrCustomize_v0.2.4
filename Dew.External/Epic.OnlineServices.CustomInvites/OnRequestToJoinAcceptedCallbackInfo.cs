namespace Epic.OnlineServices.CustomInvites;

public struct OnRequestToJoinAcceptedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnRequestToJoinAcceptedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		TargetUserId = other.TargetUserId;
		LocalUserId = other.LocalUserId;
	}
}
