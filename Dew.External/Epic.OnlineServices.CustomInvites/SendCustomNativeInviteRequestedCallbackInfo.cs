namespace Epic.OnlineServices.CustomInvites;

public struct SendCustomNativeInviteRequestedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ulong UiEventId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String TargetNativeAccountType { get; set; }

	public Utf8String TargetUserNativeAccountId { get; set; }

	public Utf8String InviteId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref SendCustomNativeInviteRequestedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		UiEventId = other.UiEventId;
		LocalUserId = other.LocalUserId;
		TargetNativeAccountType = other.TargetNativeAccountType;
		TargetUserNativeAccountId = other.TargetUserNativeAccountId;
		InviteId = other.InviteId;
	}
}
