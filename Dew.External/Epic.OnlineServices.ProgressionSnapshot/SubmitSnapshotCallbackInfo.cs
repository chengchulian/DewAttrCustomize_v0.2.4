namespace Epic.OnlineServices.ProgressionSnapshot;

public struct SubmitSnapshotCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public uint SnapshotId { get; set; }

	public object ClientData { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref SubmitSnapshotCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		SnapshotId = other.SnapshotId;
		ClientData = other.ClientData;
	}
}
