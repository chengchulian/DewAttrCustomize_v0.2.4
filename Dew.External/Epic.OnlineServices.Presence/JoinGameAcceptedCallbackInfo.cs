namespace Epic.OnlineServices.Presence;

public struct JoinGameAcceptedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String JoinInfo { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public EpicAccountId TargetUserId { get; set; }

	public ulong UiEventId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref JoinGameAcceptedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		JoinInfo = other.JoinInfo;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		UiEventId = other.UiEventId;
	}
}
