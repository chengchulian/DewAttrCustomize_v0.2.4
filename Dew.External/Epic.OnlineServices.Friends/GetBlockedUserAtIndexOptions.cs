namespace Epic.OnlineServices.Friends;

public struct GetBlockedUserAtIndexOptions
{
	public EpicAccountId LocalUserId { get; set; }

	public int Index { get; set; }
}
