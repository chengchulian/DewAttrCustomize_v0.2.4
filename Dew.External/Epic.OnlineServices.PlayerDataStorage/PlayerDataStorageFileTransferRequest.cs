using System;

namespace Epic.OnlineServices.PlayerDataStorage;

public sealed class PlayerDataStorageFileTransferRequest : Handle
{
	public PlayerDataStorageFileTransferRequest()
	{
	}

	public PlayerDataStorageFileTransferRequest(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CancelRequest()
	{
		return Bindings.EOS_PlayerDataStorageFileTransferRequest_CancelRequest(base.InnerHandle);
	}

	public Result GetFileRequestState()
	{
		return Bindings.EOS_PlayerDataStorageFileTransferRequest_GetFileRequestState(base.InnerHandle);
	}

	public Result GetFilename(out Utf8String outStringBuffer)
	{
		int outStringLength = 64;
		IntPtr outStringBufferAddress = Helper.AddAllocation(outStringLength);
		Result result = Bindings.EOS_PlayerDataStorageFileTransferRequest_GetFilename(base.InnerHandle, (uint)outStringLength, outStringBufferAddress, ref outStringLength);
		Helper.Get(outStringBufferAddress, out outStringBuffer);
		Helper.Dispose(ref outStringBufferAddress);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_PlayerDataStorageFileTransferRequest_Release(base.InnerHandle);
	}
}
