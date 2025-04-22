namespace Epic.OnlineServices.CustomInvites;

public struct RequestToJoinResponseReceivedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId FromUserId { get; set; }

	public ProductUserId ToUserId { get; set; }

	public RequestToJoinResponse Response { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref RequestToJoinResponseReceivedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		FromUserId = other.FromUserId;
		ToUserId = other.ToUserId;
		Response = other.Response;
	}
}
