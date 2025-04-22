namespace Epic.OnlineServices.UserInfo;

public struct QueryUserInfoByExternalAccountCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public Utf8String ExternalAccountId { get; set; }

	public ExternalAccountType AccountType { get; set; }

	public EpicAccountId TargetUserId { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref QueryUserInfoByExternalAccountCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		ExternalAccountId = other.ExternalAccountId;
		AccountType = other.AccountType;
		TargetUserId = other.TargetUserId;
	}
}
