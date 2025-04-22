namespace Epic.OnlineServices.Sessions;

public struct SessionInviteAcceptedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String SessionId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public Utf8String InviteId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref SessionInviteAcceptedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		SessionId = other.SessionId;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		InviteId = other.InviteId;
	}
}
