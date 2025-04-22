namespace Epic.OnlineServices.UserInfo;

public struct QueryUserInfoByDisplayNameOptions
{
	public EpicAccountId LocalUserId { get; set; }

	public Utf8String DisplayName { get; set; }
}
