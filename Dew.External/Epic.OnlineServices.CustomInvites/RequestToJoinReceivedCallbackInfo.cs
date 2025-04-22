namespace Epic.OnlineServices.CustomInvites;

public struct RequestToJoinReceivedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId FromUserId { get; set; }

	public ProductUserId ToUserId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref RequestToJoinReceivedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		FromUserId = other.FromUserId;
		ToUserId = other.ToUserId;
	}
}
