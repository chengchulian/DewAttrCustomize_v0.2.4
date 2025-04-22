using System;

namespace Epic.OnlineServices.Ecom;

public sealed class EcomInterface : Handle
{
	public const int CatalogitemApiLatest = 1;

	public const int CatalogitemEntitlementendtimestampUndefined = -1;

	public const int CatalogofferApiLatest = 5;

	public const int CatalogofferEffectivedatetimestampUndefined = -1;

	public const int CatalogofferExpirationtimestampUndefined = -1;

	public const int CatalogofferReleasedatetimestampUndefined = -1;

	public const int CatalogreleaseApiLatest = 1;

	public const int CheckoutApiLatest = 1;

	public const int CheckoutMaxEntries = 10;

	public const int CheckoutentryApiLatest = 1;

	public const int CopyentitlementbyidApiLatest = 2;

	public const int CopyentitlementbyindexApiLatest = 1;

	public const int CopyentitlementbynameandindexApiLatest = 1;

	public const int CopyitembyidApiLatest = 1;

	public const int CopyitemimageinfobyindexApiLatest = 1;

	public const int CopyitemreleasebyindexApiLatest = 1;

	public const int CopylastredeemedentitlementbyindexApiLatest = 1;

	public const int CopyofferbyidApiLatest = 3;

	public const int CopyofferbyindexApiLatest = 3;

	public const int CopyofferimageinfobyindexApiLatest = 1;

	public const int CopyofferitembyindexApiLatest = 1;

	public const int CopytransactionbyidApiLatest = 1;

	public const int CopytransactionbyindexApiLatest = 1;

	public const int EntitlementApiLatest = 2;

	public const int EntitlementEndtimestampUndefined = -1;

	public const int EntitlementidMaxLength = 32;

	public const int GetentitlementsbynamecountApiLatest = 1;

	public const int GetentitlementscountApiLatest = 1;

	public const int GetitemimageinfocountApiLatest = 1;

	public const int GetitemreleasecountApiLatest = 1;

	public const int GetlastredeemedentitlementscountApiLatest = 1;

	public const int GetoffercountApiLatest = 1;

	public const int GetofferimageinfocountApiLatest = 1;

	public const int GetofferitemcountApiLatest = 1;

	public const int GettransactioncountApiLatest = 1;

	public const int ItemownershipApiLatest = 1;

	public const int KeyimageinfoApiLatest = 1;

	public const int QueryentitlementsApiLatest = 2;

	public const int QueryentitlementsMaxEntitlementIds = 256;

	public const int QueryentitlementtokenApiLatest = 1;

	public const int QueryentitlementtokenMaxEntitlementIds = 32;

	public const int QueryoffersApiLatest = 1;

	public const int QueryownershipApiLatest = 2;

	public const int QueryownershipMaxCatalogIds = 400;

	public const int QueryownershipMaxSandboxIds = 10;

	public const int QueryownershipbysandboxidsoptionsApiLatest = 1;

	public const int QueryownershiptokenApiLatest = 2;

	public const int QueryownershiptokenMaxCatalogitemIds = 32;

	public const int RedeementitlementsApiLatest = 2;

	public const int RedeementitlementsMaxIds = 32;

	public const int TransactionidMaximumLength = 64;

	public EcomInterface()
	{
	}

	public EcomInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public void Checkout(ref CheckoutOptions options, object clientData, OnCheckoutCallback completionDelegate)
	{
		CheckoutOptionsInternal optionsInternal = default(CheckoutOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnCheckoutCallbackInternal completionDelegateInternal = OnCheckoutCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Ecom_Checkout(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result CopyEntitlementById(ref CopyEntitlementByIdOptions options, out Entitlement? outEntitlement)
	{
		CopyEntitlementByIdOptionsInternal optionsInternal = default(CopyEntitlementByIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outEntitlementAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyEntitlementById(base.InnerHandle, ref optionsInternal, ref outEntitlementAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<EntitlementInternal, Entitlement>(outEntitlementAddress, out outEntitlement);
		if (outEntitlement.HasValue)
		{
			Bindings.EOS_Ecom_Entitlement_Release(outEntitlementAddress);
		}
		return result;
	}

	public Result CopyEntitlementByIndex(ref CopyEntitlementByIndexOptions options, out Entitlement? outEntitlement)
	{
		CopyEntitlementByIndexOptionsInternal optionsInternal = default(CopyEntitlementByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outEntitlementAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyEntitlementByIndex(base.InnerHandle, ref optionsInternal, ref outEntitlementAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<EntitlementInternal, Entitlement>(outEntitlementAddress, out outEntitlement);
		if (outEntitlement.HasValue)
		{
			Bindings.EOS_Ecom_Entitlement_Release(outEntitlementAddress);
		}
		return result;
	}

	public Result CopyEntitlementByNameAndIndex(ref CopyEntitlementByNameAndIndexOptions options, out Entitlement? outEntitlement)
	{
		CopyEntitlementByNameAndIndexOptionsInternal optionsInternal = default(CopyEntitlementByNameAndIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outEntitlementAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyEntitlementByNameAndIndex(base.InnerHandle, ref optionsInternal, ref outEntitlementAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<EntitlementInternal, Entitlement>(outEntitlementAddress, out outEntitlement);
		if (outEntitlement.HasValue)
		{
			Bindings.EOS_Ecom_Entitlement_Release(outEntitlementAddress);
		}
		return result;
	}

	public Result CopyItemById(ref CopyItemByIdOptions options, out CatalogItem? outItem)
	{
		CopyItemByIdOptionsInternal optionsInternal = default(CopyItemByIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outItemAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyItemById(base.InnerHandle, ref optionsInternal, ref outItemAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<CatalogItemInternal, CatalogItem>(outItemAddress, out outItem);
		if (outItem.HasValue)
		{
			Bindings.EOS_Ecom_CatalogItem_Release(outItemAddress);
		}
		return result;
	}

	public Result CopyItemImageInfoByIndex(ref CopyItemImageInfoByIndexOptions options, out KeyImageInfo? outImageInfo)
	{
		CopyItemImageInfoByIndexOptionsInternal optionsInternal = default(CopyItemImageInfoByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outImageInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyItemImageInfoByIndex(base.InnerHandle, ref optionsInternal, ref outImageInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<KeyImageInfoInternal, KeyImageInfo>(outImageInfoAddress, out outImageInfo);
		if (outImageInfo.HasValue)
		{
			Bindings.EOS_Ecom_KeyImageInfo_Release(outImageInfoAddress);
		}
		return result;
	}

	public Result CopyItemReleaseByIndex(ref CopyItemReleaseByIndexOptions options, out CatalogRelease? outRelease)
	{
		CopyItemReleaseByIndexOptionsInternal optionsInternal = default(CopyItemReleaseByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outReleaseAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyItemReleaseByIndex(base.InnerHandle, ref optionsInternal, ref outReleaseAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<CatalogReleaseInternal, CatalogRelease>(outReleaseAddress, out outRelease);
		if (outRelease.HasValue)
		{
			Bindings.EOS_Ecom_CatalogRelease_Release(outReleaseAddress);
		}
		return result;
	}

	public Result CopyLastRedeemedEntitlementByIndex(ref CopyLastRedeemedEntitlementByIndexOptions options, out Utf8String outRedeemedEntitlementId)
	{
		CopyLastRedeemedEntitlementByIndexOptionsInternal optionsInternal = default(CopyLastRedeemedEntitlementByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		int inOutRedeemedEntitlementIdLength = 33;
		IntPtr outRedeemedEntitlementIdAddress = Helper.AddAllocation(inOutRedeemedEntitlementIdLength);
		Result result = Bindings.EOS_Ecom_CopyLastRedeemedEntitlementByIndex(base.InnerHandle, ref optionsInternal, outRedeemedEntitlementIdAddress, ref inOutRedeemedEntitlementIdLength);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outRedeemedEntitlementIdAddress, out outRedeemedEntitlementId);
		Helper.Dispose(ref outRedeemedEntitlementIdAddress);
		return result;
	}

	public Result CopyOfferById(ref CopyOfferByIdOptions options, out CatalogOffer? outOffer)
	{
		CopyOfferByIdOptionsInternal optionsInternal = default(CopyOfferByIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outOfferAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyOfferById(base.InnerHandle, ref optionsInternal, ref outOfferAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<CatalogOfferInternal, CatalogOffer>(outOfferAddress, out outOffer);
		if (outOffer.HasValue)
		{
			Bindings.EOS_Ecom_CatalogOffer_Release(outOfferAddress);
		}
		return result;
	}

	public Result CopyOfferByIndex(ref CopyOfferByIndexOptions options, out CatalogOffer? outOffer)
	{
		CopyOfferByIndexOptionsInternal optionsInternal = default(CopyOfferByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outOfferAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyOfferByIndex(base.InnerHandle, ref optionsInternal, ref outOfferAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<CatalogOfferInternal, CatalogOffer>(outOfferAddress, out outOffer);
		if (outOffer.HasValue)
		{
			Bindings.EOS_Ecom_CatalogOffer_Release(outOfferAddress);
		}
		return result;
	}

	public Result CopyOfferImageInfoByIndex(ref CopyOfferImageInfoByIndexOptions options, out KeyImageInfo? outImageInfo)
	{
		CopyOfferImageInfoByIndexOptionsInternal optionsInternal = default(CopyOfferImageInfoByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outImageInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyOfferImageInfoByIndex(base.InnerHandle, ref optionsInternal, ref outImageInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<KeyImageInfoInternal, KeyImageInfo>(outImageInfoAddress, out outImageInfo);
		if (outImageInfo.HasValue)
		{
			Bindings.EOS_Ecom_KeyImageInfo_Release(outImageInfoAddress);
		}
		return result;
	}

	public Result CopyOfferItemByIndex(ref CopyOfferItemByIndexOptions options, out CatalogItem? outItem)
	{
		CopyOfferItemByIndexOptionsInternal optionsInternal = default(CopyOfferItemByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outItemAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyOfferItemByIndex(base.InnerHandle, ref optionsInternal, ref outItemAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<CatalogItemInternal, CatalogItem>(outItemAddress, out outItem);
		if (outItem.HasValue)
		{
			Bindings.EOS_Ecom_CatalogItem_Release(outItemAddress);
		}
		return result;
	}

	public Result CopyTransactionById(ref CopyTransactionByIdOptions options, out Transaction outTransaction)
	{
		CopyTransactionByIdOptionsInternal optionsInternal = default(CopyTransactionByIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outTransactionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyTransactionById(base.InnerHandle, ref optionsInternal, ref outTransactionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outTransactionAddress, out outTransaction);
		return result;
	}

	public Result CopyTransactionByIndex(ref CopyTransactionByIndexOptions options, out Transaction outTransaction)
	{
		CopyTransactionByIndexOptionsInternal optionsInternal = default(CopyTransactionByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outTransactionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Ecom_CopyTransactionByIndex(base.InnerHandle, ref optionsInternal, ref outTransactionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outTransactionAddress, out outTransaction);
		return result;
	}

	public uint GetEntitlementsByNameCount(ref GetEntitlementsByNameCountOptions options)
	{
		GetEntitlementsByNameCountOptionsInternal optionsInternal = default(GetEntitlementsByNameCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetEntitlementsByNameCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetEntitlementsCount(ref GetEntitlementsCountOptions options)
	{
		GetEntitlementsCountOptionsInternal optionsInternal = default(GetEntitlementsCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetEntitlementsCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetItemImageInfoCount(ref GetItemImageInfoCountOptions options)
	{
		GetItemImageInfoCountOptionsInternal optionsInternal = default(GetItemImageInfoCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetItemImageInfoCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetItemReleaseCount(ref GetItemReleaseCountOptions options)
	{
		GetItemReleaseCountOptionsInternal optionsInternal = default(GetItemReleaseCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetItemReleaseCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetLastRedeemedEntitlementsCount(ref GetLastRedeemedEntitlementsCountOptions options)
	{
		GetLastRedeemedEntitlementsCountOptionsInternal optionsInternal = default(GetLastRedeemedEntitlementsCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetLastRedeemedEntitlementsCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetOfferCount(ref GetOfferCountOptions options)
	{
		GetOfferCountOptionsInternal optionsInternal = default(GetOfferCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetOfferCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetOfferImageInfoCount(ref GetOfferImageInfoCountOptions options)
	{
		GetOfferImageInfoCountOptionsInternal optionsInternal = default(GetOfferImageInfoCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetOfferImageInfoCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetOfferItemCount(ref GetOfferItemCountOptions options)
	{
		GetOfferItemCountOptionsInternal optionsInternal = default(GetOfferItemCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetOfferItemCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetTransactionCount(ref GetTransactionCountOptions options)
	{
		GetTransactionCountOptionsInternal optionsInternal = default(GetTransactionCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Ecom_GetTransactionCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryEntitlementToken(ref QueryEntitlementTokenOptions options, object clientData, OnQueryEntitlementTokenCallback completionDelegate)
	{
		QueryEntitlementTokenOptionsInternal optionsInternal = default(QueryEntitlementTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryEntitlementTokenCallbackInternal completionDelegateInternal = OnQueryEntitlementTokenCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Ecom_QueryEntitlementToken(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryEntitlements(ref QueryEntitlementsOptions options, object clientData, OnQueryEntitlementsCallback completionDelegate)
	{
		QueryEntitlementsOptionsInternal optionsInternal = default(QueryEntitlementsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryEntitlementsCallbackInternal completionDelegateInternal = OnQueryEntitlementsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Ecom_QueryEntitlements(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryOffers(ref QueryOffersOptions options, object clientData, OnQueryOffersCallback completionDelegate)
	{
		QueryOffersOptionsInternal optionsInternal = default(QueryOffersOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryOffersCallbackInternal completionDelegateInternal = OnQueryOffersCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Ecom_QueryOffers(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryOwnership(ref QueryOwnershipOptions options, object clientData, OnQueryOwnershipCallback completionDelegate)
	{
		QueryOwnershipOptionsInternal optionsInternal = default(QueryOwnershipOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryOwnershipCallbackInternal completionDelegateInternal = OnQueryOwnershipCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Ecom_QueryOwnership(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryOwnershipBySandboxIds(ref QueryOwnershipBySandboxIdsOptions options, object clientData, OnQueryOwnershipBySandboxIdsCallback completionDelegate)
	{
		QueryOwnershipBySandboxIdsOptionsInternal optionsInternal = default(QueryOwnershipBySandboxIdsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryOwnershipBySandboxIdsCallbackInternal completionDelegateInternal = OnQueryOwnershipBySandboxIdsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Ecom_QueryOwnershipBySandboxIds(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryOwnershipToken(ref QueryOwnershipTokenOptions options, object clientData, OnQueryOwnershipTokenCallback completionDelegate)
	{
		QueryOwnershipTokenOptionsInternal optionsInternal = default(QueryOwnershipTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryOwnershipTokenCallbackInternal completionDelegateInternal = OnQueryOwnershipTokenCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Ecom_QueryOwnershipToken(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RedeemEntitlements(ref RedeemEntitlementsOptions options, object clientData, OnRedeemEntitlementsCallback completionDelegate)
	{
		RedeemEntitlementsOptionsInternal optionsInternal = default(RedeemEntitlementsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRedeemEntitlementsCallbackInternal completionDelegateInternal = OnRedeemEntitlementsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Ecom_RedeemEntitlements(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnCheckoutCallbackInternal))]
	internal static void OnCheckoutCallbackInternalImplementation(ref CheckoutCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<CheckoutCallbackInfoInternal, OnCheckoutCallback, CheckoutCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryEntitlementTokenCallbackInternal))]
	internal static void OnQueryEntitlementTokenCallbackInternalImplementation(ref QueryEntitlementTokenCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryEntitlementTokenCallbackInfoInternal, OnQueryEntitlementTokenCallback, QueryEntitlementTokenCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryEntitlementsCallbackInternal))]
	internal static void OnQueryEntitlementsCallbackInternalImplementation(ref QueryEntitlementsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryEntitlementsCallbackInfoInternal, OnQueryEntitlementsCallback, QueryEntitlementsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryOffersCallbackInternal))]
	internal static void OnQueryOffersCallbackInternalImplementation(ref QueryOffersCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryOffersCallbackInfoInternal, OnQueryOffersCallback, QueryOffersCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryOwnershipBySandboxIdsCallbackInternal))]
	internal static void OnQueryOwnershipBySandboxIdsCallbackInternalImplementation(ref QueryOwnershipBySandboxIdsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryOwnershipBySandboxIdsCallbackInfoInternal, OnQueryOwnershipBySandboxIdsCallback, QueryOwnershipBySandboxIdsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryOwnershipCallbackInternal))]
	internal static void OnQueryOwnershipCallbackInternalImplementation(ref QueryOwnershipCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryOwnershipCallbackInfoInternal, OnQueryOwnershipCallback, QueryOwnershipCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryOwnershipTokenCallbackInternal))]
	internal static void OnQueryOwnershipTokenCallbackInternalImplementation(ref QueryOwnershipTokenCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryOwnershipTokenCallbackInfoInternal, OnQueryOwnershipTokenCallback, QueryOwnershipTokenCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRedeemEntitlementsCallbackInternal))]
	internal static void OnRedeemEntitlementsCallbackInternalImplementation(ref RedeemEntitlementsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<RedeemEntitlementsCallbackInfoInternal, OnRedeemEntitlementsCallback, RedeemEntitlementsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
