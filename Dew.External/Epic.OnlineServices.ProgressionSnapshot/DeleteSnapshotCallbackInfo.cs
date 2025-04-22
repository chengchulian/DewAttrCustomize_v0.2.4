namespace Epic.OnlineServices.ProgressionSnapshot;

public struct DeleteSnapshotCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public object ClientData { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref DeleteSnapshotCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		LocalUserId = other.LocalUserId;
		ClientData = other.ClientData;
	}
}
