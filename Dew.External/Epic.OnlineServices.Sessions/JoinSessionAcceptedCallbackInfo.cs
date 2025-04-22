namespace Epic.OnlineServices.Sessions;

public struct JoinSessionAcceptedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public ulong UiEventId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref JoinSessionAcceptedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		UiEventId = other.UiEventId;
	}
}
