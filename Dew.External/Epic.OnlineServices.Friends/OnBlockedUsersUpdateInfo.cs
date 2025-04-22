namespace Epic.OnlineServices.Friends;

public struct OnBlockedUsersUpdateInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public EpicAccountId TargetUserId { get; set; }

	public bool Blocked { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnBlockedUsersUpdateInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		Blocked = other.Blocked;
	}
}
