namespace Epic.OnlineServices.CustomInvites;

public struct OnCustomInviteReceivedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String CustomInviteId { get; set; }

	public Utf8String Payload { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnCustomInviteReceivedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		TargetUserId = other.TargetUserId;
		LocalUserId = other.LocalUserId;
		CustomInviteId = other.CustomInviteId;
		Payload = other.Payload;
	}
}
