namespace Epic.OnlineServices.Friends;

public struct OnFriendsUpdateInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public EpicAccountId TargetUserId { get; set; }

	public FriendsStatus PreviousStatus { get; set; }

	public FriendsStatus CurrentStatus { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnFriendsUpdateInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		PreviousStatus = other.PreviousStatus;
		CurrentStatus = other.CurrentStatus;
	}
}
