using System;

namespace Epic.OnlineServices;

public sealed class Common
{
	public const ulong InvalidNotificationid = 0uL;

	public static readonly Utf8String IptUnknown = (string)null;

	public const int OptEpic = 100;

	public const int OptSteam = 4000;

	public const int OptUnknown = 0;

	public const int PagequeryApiLatest = 1;

	public const int PagequeryMaxcountDefault = 10;

	public const int PagequeryMaxcountMaximum = 100;

	public const int PaginationApiLatest = 1;

	public static bool IsOperationComplete(Result result)
	{
		Helper.Get(Bindings.EOS_EResult_IsOperationComplete(result), out var funcResultReturn);
		return funcResultReturn;
	}

	public static Utf8String ToString(Result result)
	{
		Helper.Get(Bindings.EOS_EResult_ToString(result), out Utf8String funcResultReturn);
		return funcResultReturn;
	}

	public static Result ToString(ArraySegment<byte> byteArray, out Utf8String outBuffer)
	{
		IntPtr byteArrayAddress = IntPtr.Zero;
		Helper.Set(byteArray, ref byteArrayAddress, out var length);
		uint inOutBufferLength = 10240u;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_ByteArray_ToString(byteArrayAddress, length, outBufferAddress, ref inOutBufferLength);
		Helper.Dispose(ref byteArrayAddress);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public static Utf8String ToString(ArraySegment<byte> byteArray)
	{
		ToString(byteArray, out var funcResult);
		return funcResult;
	}
}
