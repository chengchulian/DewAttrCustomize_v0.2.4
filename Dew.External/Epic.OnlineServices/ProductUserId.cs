using System;

namespace Epic.OnlineServices;

public sealed class ProductUserId : Handle
{
	public const int ProductuseridMaxLength = 32;

	public ProductUserId()
	{
	}

	public ProductUserId(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public static ProductUserId FromString(Utf8String productUserIdString)
	{
		IntPtr productUserIdStringAddress = IntPtr.Zero;
		Helper.Set(productUserIdString, ref productUserIdStringAddress);
		IntPtr from = Bindings.EOS_ProductUserId_FromString(productUserIdStringAddress);
		Helper.Dispose(ref productUserIdStringAddress);
		Helper.Get(from, out ProductUserId funcResultReturn);
		return funcResultReturn;
	}

	public static explicit operator ProductUserId(Utf8String value)
	{
		return FromString(value);
	}

	public bool IsValid()
	{
		Helper.Get(Bindings.EOS_ProductUserId_IsValid(base.InnerHandle), out var funcResultReturn);
		return funcResultReturn;
	}

	public Result ToString(out Utf8String outBuffer)
	{
		int inOutBufferLength = 33;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_ProductUserId_ToString(base.InnerHandle, outBufferAddress, ref inOutBufferLength);
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

	public static explicit operator Utf8String(ProductUserId value)
	{
		Utf8String result = null;
		if (value != null)
		{
			value.ToString(out result);
		}
		return result;
	}
}
