namespace Epic.OnlineServices.Ecom;

public struct QueryOwnershipBySandboxIdsCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public EpicAccountId LocalUserId { get; set; }

	public SandboxIdItemOwnership[] SandboxIdItemOwnerships { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref QueryOwnershipBySandboxIdsCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		SandboxIdItemOwnerships = other.SandboxIdItemOwnerships;
	}
}
