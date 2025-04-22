namespace Epic.OnlineServices.Presence;

public struct PresenceChangedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public EpicAccountId PresenceUserId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref PresenceChangedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		PresenceUserId = other.PresenceUserId;
	}
}
