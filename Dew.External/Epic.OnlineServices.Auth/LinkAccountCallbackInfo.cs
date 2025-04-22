namespace Epic.OnlineServices.Auth;

public struct LinkAccountCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public PinGrantInfo? PinGrantInfo { get; set; }

	public EpicAccountId SelectedAccountId { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref LinkAccountCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		PinGrantInfo = other.PinGrantInfo;
		SelectedAccountId = other.SelectedAccountId;
	}
}
