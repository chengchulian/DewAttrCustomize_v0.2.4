namespace Epic.OnlineServices.IntegratedPlatform;

public struct UserLoginStatusChangedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Utf8String PlatformType { get; set; }

	public Utf8String LocalPlatformUserId { get; set; }

	public EpicAccountId AccountId { get; set; }

	public ProductUserId ProductUserId { get; set; }

	public LoginStatus PreviousLoginStatus { get; set; }

	public LoginStatus CurrentLoginStatus { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref UserLoginStatusChangedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		PlatformType = other.PlatformType;
		LocalPlatformUserId = other.LocalPlatformUserId;
		AccountId = other.AccountId;
		ProductUserId = other.ProductUserId;
		PreviousLoginStatus = other.PreviousLoginStatus;
		CurrentLoginStatus = other.CurrentLoginStatus;
	}
}
