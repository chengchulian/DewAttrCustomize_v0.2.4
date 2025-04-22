using System;

namespace Epic.OnlineServices.Ecom;

public sealed class Transaction : Handle
{
	public const int TransactionCopyentitlementbyindexApiLatest = 1;

	public const int TransactionGetentitlementscountApiLatest = 1;

	public Transaction()
	{
	}

	public Transaction(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyEntitlementByIndex(ref TransactionCopyEntitlementByIndexOptions options, out Entitlement? outEntitlement)
	{
		TransactionCopyEntitlementByIndexOptionsInternal optionsInternal = default(TransactionCopyEntitlementByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outEntitlementAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_Transaction_CopyEntitlementByIndex(base.InnerHandle, ref optionsInternal, ref outEntitlementAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<EntitlementInternal, Entitlement>(outEntitlementAddress, out outEntitlement);
		if (outEntitlement.HasValue)
		{
			Bindings.EOS_Ecom_Entitlement_Release(outEntitlementAddress);
		}
		return result;
	}

	public uint GetEntitlementsCount(ref TransactionGetEntitlementsCountOptions options)
	{
		TransactionGetEntitlementsCountOptionsInternal optionsInternal = default(TransactionGetEntitlementsCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_Transaction_GetEntitlementsCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetTransactionId(out Utf8String outBuffer)
	{
		int inOutBufferLength = 65;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Ecom_Transaction_GetTransactionId(base.InnerHandle, outBufferAddress, ref inOutBufferLength);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public void Release()
	{
		Bindings.EOS_Ecom_Transaction_Release(base.InnerHandle);
	}
}
