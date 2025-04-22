using System;

namespace Epic.OnlineServices.TitleStorage;

public struct ReadFileDataCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String Filename { get; set; }

	public uint TotalFileSizeBytes { get; set; }

	public bool IsLastChunk { get; set; }

	public ArraySegment<byte> DataChunk { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref ReadFileDataCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		Filename = other.Filename;
		TotalFileSizeBytes = other.TotalFileSizeBytes;
		IsLastChunk = other.IsLastChunk;
		DataChunk = other.DataChunk;
	}
}
