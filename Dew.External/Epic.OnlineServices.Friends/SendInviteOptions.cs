namespace Epic.OnlineServices.Friends;

public struct SendInviteOptions
{
	public EpicAccountId LocalUserId { get; set; }

	public EpicAccountId TargetUserId { get; set; }
}
