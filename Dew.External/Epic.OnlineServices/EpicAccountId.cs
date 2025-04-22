using System;

namespace Epic.OnlineServices;

public sealed class EpicAccountId : Handle
{
	public const int EpicaccountidMaxLength = 32;

	public EpicAccountId()
	{
	}

	public EpicAccountId(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public static EpicAccountId FromString(Utf8String accountIdString)
	{
		IntPtr accountIdStringAddress = IntPtr.Zero;
		Helper.Set(accountIdString, ref accountIdStringAddress);
		IntPtr from = Bindings.EOS_EpicAccountId_FromString(accountIdStringAddress);
		Helper.Dispose(ref accountIdStringAddress);
		Helper.Get(from, out EpicAccountId funcResultReturn);
		return funcResultReturn;
	}

	public static explicit operator EpicAccountId(Utf8String value)
	{
		return FromString(value);
	}

	public bool IsValid()
	{
		Helper.Get(Bindings.EOS_EpicAccountId_IsValid(base.InnerHandle), out var funcResultReturn);
		return funcResultReturn;
	}

	public Result ToString(out Utf8String outBuffer)
	{
		int inOutBufferLength = 33;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_EpicAccountId_ToString(base.InnerHandle, outBufferAddress, ref inOutBufferLength);
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

	public static explicit operator Utf8String(EpicAccountId value)
	{
		Utf8String result = null;
		if (value != null)
		{
			value.ToString(out result);
		}
		return result;
	}
}
