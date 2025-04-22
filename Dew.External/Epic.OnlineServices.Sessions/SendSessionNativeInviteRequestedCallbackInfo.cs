namespace Epic.OnlineServices.Sessions;

public struct SendSessionNativeInviteRequestedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ulong UiEventId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String TargetNativeAccountType { get; set; }

	public Utf8String TargetUserNativeAccountId { get; set; }

	public Utf8String SessionId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref SendSessionNativeInviteRequestedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		UiEventId = other.UiEventId;
		LocalUserId = other.LocalUserId;
		TargetNativeAccountType = other.TargetNativeAccountType;
		TargetUserNativeAccountId = other.TargetUserNativeAccountId;
		SessionId = other.SessionId;
	}
}
