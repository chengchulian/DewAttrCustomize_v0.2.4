namespace Epic.OnlineServices.Sessions;

public struct SessionInviteRejectedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String InviteId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public Utf8String SessionId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref SessionInviteRejectedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		InviteId = other.InviteId;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		SessionId = other.SessionId;
	}
}
