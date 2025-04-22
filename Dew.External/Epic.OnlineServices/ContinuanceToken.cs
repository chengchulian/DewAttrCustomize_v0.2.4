using System;

namespace Epic.OnlineServices;

public sealed class ContinuanceToken : Handle
{
	public ContinuanceToken()
	{
	}

	public ContinuanceToken(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result ToString(out Utf8String outBuffer)
	{
		int inOutBufferLength = 1024;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_ContinuanceToken_ToString(base.InnerHandle, outBufferAddress, ref inOutBufferLength);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public override string ToString()
	{
		ToString(out var funcResult);
		return funcResult;
	}

	public override string ToString(string format, IFormatProvider formatProvider)
	{
		if (format != null)
		{
			return string.Format(format, ToString());
		}
		return ToString();
	}

	public static explicit operator Utf8String(ContinuanceToken value)
	{
		Utf8String result = null;
		if (value != null)
		{
			value.ToString(out result);
		}
		return result;
	}
}
