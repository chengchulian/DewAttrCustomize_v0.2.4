using System;

namespace Epic.OnlineServices.Connect;

public struct ExternalAccountInfo
{
	public ProductUserId ProductUserId { get; set; }

	public Utf8String DisplayName { get; set; }

	public Utf8String AccountId { get; set; }

	public ExternalAccountType AccountIdType { get; set; }

	public DateTimeOffset? LastLoginTime { get; set; }

	internal void Set(ref ExternalAccountInfoInternal other)
	{
		ProductUserId = other.ProductUserId;
		DisplayName = other.DisplayName;
		AccountId = other.AccountId;
		AccountIdType = other.AccountIdType;
		LastLoginTime = other.LastLoginTime;
	}
}
