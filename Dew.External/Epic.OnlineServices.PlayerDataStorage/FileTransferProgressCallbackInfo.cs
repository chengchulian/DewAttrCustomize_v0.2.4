namespace Epic.OnlineServices.PlayerDataStorage;

public struct FileTransferProgressCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String Filename { get; set; }

	public uint BytesTransferred { get; set; }

	public uint TotalFileSizeBytes { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref FileTransferProgressCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		Filename = other.Filename;
		BytesTransferred = other.BytesTransferred;
		TotalFileSizeBytes = other.TotalFileSizeBytes;
	}
}
