namespace Epic.OnlineServices.UserInfo;

public struct UserInfoData
{
	public EpicAccountId UserId { get; set; }

	public Utf8String Country { get; set; }

	public Utf8String DisplayName { get; set; }

	public Utf8String PreferredLanguage { get; set; }

	public Utf8String Nickname { get; set; }

	public Utf8String DisplayNameSanitized { get; set; }

	internal void Set(ref UserInfoDataInternal other)
	{
		UserId = other.UserId;
		Country = other.Country;
		DisplayName = other.DisplayName;
		PreferredLanguage = other.PreferredLanguage;
		Nickname = other.Nickname;
		DisplayNameSanitized = other.DisplayNameSanitized;
	}
}
