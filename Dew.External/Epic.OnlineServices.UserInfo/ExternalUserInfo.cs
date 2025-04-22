namespace Epic.OnlineServices.UserInfo;

public struct ExternalUserInfo
{
	public ExternalAccountType AccountType { get; set; }

	public Utf8String AccountId { get; set; }

	public Utf8String DisplayName { get; set; }

	public Utf8String DisplayNameSanitized { get; set; }

	internal void Set(ref ExternalUserInfoInternal other)
	{
		AccountType = other.AccountType;
		AccountId = other.AccountId;
		DisplayName = other.DisplayName;
		DisplayNameSanitized = other.DisplayNameSanitized;
	}
}
