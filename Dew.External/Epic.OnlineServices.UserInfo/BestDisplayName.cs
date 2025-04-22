namespace Epic.OnlineServices.UserInfo;

public struct BestDisplayName
{
	public EpicAccountId UserId { get; set; }

	public Utf8String DisplayName { get; set; }

	public Utf8String DisplayNameSanitized { get; set; }

	public Utf8String Nickname { get; set; }

	public uint PlatformType { get; set; }

	internal void Set(ref BestDisplayNameInternal other)
	{
		UserId = other.UserId;
		DisplayName = other.DisplayName;
		DisplayNameSanitized = other.DisplayNameSanitized;
		Nickname = other.Nickname;
		PlatformType = other.PlatformType;
	}
}
