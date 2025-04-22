namespace Epic.OnlineServices.UserInfo;

public struct QueryUserInfoByDisplayNameCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public EpicAccountId TargetUserId { get; set; }

	public Utf8String DisplayName { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref QueryUserInfoByDisplayNameCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		DisplayName = other.DisplayName;
	}
}
