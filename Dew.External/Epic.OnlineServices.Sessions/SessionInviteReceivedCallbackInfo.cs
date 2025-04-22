namespace Epic.OnlineServices.Sessions;

public struct SessionInviteReceivedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public Utf8String InviteId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref SessionInviteReceivedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		InviteId = other.InviteId;
	}
}
