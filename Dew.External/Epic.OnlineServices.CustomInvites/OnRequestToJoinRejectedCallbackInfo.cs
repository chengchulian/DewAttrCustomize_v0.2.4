namespace Epic.OnlineServices.CustomInvites;

public struct OnRequestToJoinRejectedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnRequestToJoinRejectedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		TargetUserId = other.TargetUserId;
		LocalUserId = other.LocalUserId;
	}
}
