namespace Epic.OnlineServices.Auth;

public struct LoginStatusChangedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public LoginStatus PrevStatus { get; set; }

	public LoginStatus CurrentStatus { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref LoginStatusChangedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		PrevStatus = other.PrevStatus;
		CurrentStatus = other.CurrentStatus;
	}
}
