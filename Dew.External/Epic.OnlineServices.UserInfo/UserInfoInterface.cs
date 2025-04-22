using System;

namespace Epic.OnlineServices.UserInfo;

public sealed class UserInfoInterface : Handle
{
	public const int BestdisplaynameApiLatest = 1;

	public const int CopybestdisplaynameApiLatest = 1;

	public const int CopybestdisplaynamewithplatformApiLatest = 1;

	public const int CopyexternaluserinfobyaccountidApiLatest = 1;

	public const int CopyexternaluserinfobyaccounttypeApiLatest = 1;

	public const int CopyexternaluserinfobyindexApiLatest = 1;

	public const int CopyuserinfoApiLatest = 3;

	public const int ExternaluserinfoApiLatest = 2;

	public const int GetexternaluserinfocountApiLatest = 1;

	public const int GetlocalplatformtypeApiLatest = 1;

	public const int MaxDisplaynameCharacters = 16;

	public const int MaxDisplaynameUtf8Length = 64;

	public const int QueryuserinfoApiLatest = 1;

	public const int QueryuserinfobydisplaynameApiLatest = 1;

	public const int QueryuserinfobyexternalaccountApiLatest = 1;

	public UserInfoInterface()
	{
	}

	public UserInfoInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result CopyBestDisplayName(ref CopyBestDisplayNameOptions options, out BestDisplayName? outBestDisplayName)
	{
		CopyBestDisplayNameOptionsInternal optionsInternal = default(CopyBestDisplayNameOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outBestDisplayNameAddress = IntPtr.Zero;
		Result result = Bindings.EOS_UserInfo_CopyBestDisplayName(base.InnerHandle, ref optionsInternal, ref outBestDisplayNameAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<BestDisplayNameInternal, BestDisplayName>(outBestDisplayNameAddress, out outBestDisplayName);
		if (outBestDisplayName.HasValue)
		{
			Bindings.EOS_UserInfo_BestDisplayName_Release(outBestDisplayNameAddress);
		}
		return result;
	}

	public Result CopyBestDisplayNameWithPlatform(ref CopyBestDisplayNameWithPlatformOptions options, out BestDisplayName? outBestDisplayName)
	{
		CopyBestDisplayNameWithPlatformOptionsInternal optionsInternal = default(CopyBestDisplayNameWithPlatformOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outBestDisplayNameAddress = IntPtr.Zero;
		Result result = Bindings.EOS_UserInfo_CopyBestDisplayNameWithPlatform(base.InnerHandle, ref optionsInternal, ref outBestDisplayNameAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<BestDisplayNameInternal, BestDisplayName>(outBestDisplayNameAddress, out outBestDisplayName);
		if (outBestDisplayName.HasValue)
		{
			Bindings.EOS_UserInfo_BestDisplayName_Release(outBestDisplayNameAddress);
		}
		return result;
	}

	public Result CopyExternalUserInfoByAccountId(ref CopyExternalUserInfoByAccountIdOptions options, out ExternalUserInfo? outExternalUserInfo)
	{
		CopyExternalUserInfoByAccountIdOptionsInternal optionsInternal = default(CopyExternalUserInfoByAccountIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outExternalUserInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_UserInfo_CopyExternalUserInfoByAccountId(base.InnerHandle, ref optionsInternal, ref outExternalUserInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ExternalUserInfoInternal, ExternalUserInfo>(outExternalUserInfoAddress, out outExternalUserInfo);
		if (outExternalUserInfo.HasValue)
		{
			Bindings.EOS_UserInfo_ExternalUserInfo_Release(outExternalUserInfoAddress);
		}
		return result;
	}

	public Result CopyExternalUserInfoByAccountType(ref CopyExternalUserInfoByAccountTypeOptions options, out ExternalUserInfo? outExternalUserInfo)
	{
		CopyExternalUserInfoByAccountTypeOptionsInternal optionsInternal = default(CopyExternalUserInfoByAccountTypeOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outExternalUserInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_UserInfo_CopyExternalUserInfoByAccountType(base.InnerHandle, ref optionsInternal, ref outExternalUserInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ExternalUserInfoInternal, ExternalUserInfo>(outExternalUserInfoAddress, out outExternalUserInfo);
		if (outExternalUserInfo.HasValue)
		{
			Bindings.EOS_UserInfo_ExternalUserInfo_Release(outExternalUserInfoAddress);
		}
		return result;
	}

	public Result CopyExternalUserInfoByIndex(ref CopyExternalUserInfoByIndexOptions options, out ExternalUserInfo? outExternalUserInfo)
	{
		CopyExternalUserInfoByIndexOptionsInternal optionsInternal = default(CopyExternalUserInfoByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outExternalUserInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_UserInfo_CopyExternalUserInfoByIndex(base.InnerHandle, ref optionsInternal, ref outExternalUserInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ExternalUserInfoInternal, ExternalUserInfo>(outExternalUserInfoAddress, out outExternalUserInfo);
		if (outExternalUserInfo.HasValue)
		{
			Bindings.EOS_UserInfo_ExternalUserInfo_Release(outExternalUserInfoAddress);
		}
		return result;
	}

	public Result CopyUserInfo(ref CopyUserInfoOptions options, out UserInfoData? outUserInfo)
	{
		CopyUserInfoOptionsInternal optionsInternal = default(CopyUserInfoOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outUserInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_UserInfo_CopyUserInfo(base.InnerHandle, ref optionsInternal, ref outUserInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<UserInfoDataInternal, UserInfoData>(outUserInfoAddress, out outUserInfo);
		if (outUserInfo.HasValue)
		{
			Bindings.EOS_UserInfo_Release(outUserInfoAddress);
		}
		return result;
	}

	public uint GetExternalUserInfoCount(ref GetExternalUserInfoCountOptions options)
	{
		GetExternalUserInfoCountOptionsInternal optionsInternal = default(GetExternalUserInfoCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_UserInfo_GetExternalUserInfoCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetLocalPlatformType(ref GetLocalPlatformTypeOptions options)
	{
		GetLocalPlatformTypeOptionsInternal optionsInternal = default(GetLocalPlatformTypeOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_UserInfo_GetLocalPlatformType(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryUserInfo(ref QueryUserInfoOptions options, object clientData, OnQueryUserInfoCallback completionDelegate)
	{
		QueryUserInfoOptionsInternal optionsInternal = default(QueryUserInfoOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryUserInfoCallbackInternal completionDelegateInternal = OnQueryUserInfoCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_UserInfo_QueryUserInfo(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryUserInfoByDisplayName(ref QueryUserInfoByDisplayNameOptions options, object clientData, OnQueryUserInfoByDisplayNameCallback completionDelegate)
	{
		QueryUserInfoByDisplayNameOptionsInternal optionsInternal = default(QueryUserInfoByDisplayNameOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryUserInfoByDisplayNameCallbackInternal completionDelegateInternal = OnQueryUserInfoByDisplayNameCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_UserInfo_QueryUserInfoByDisplayName(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryUserInfoByExternalAccount(ref QueryUserInfoByExternalAccountOptions options, object clientData, OnQueryUserInfoByExternalAccountCallback completionDelegate)
	{
		QueryUserInfoByExternalAccountOptionsInternal optionsInternal = default(QueryUserInfoByExternalAccountOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryUserInfoByExternalAccountCallbackInternal completionDelegateInternal = OnQueryUserInfoByExternalAccountCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_UserInfo_QueryUserInfoByExternalAccount(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnQueryUserInfoByDisplayNameCallbackInternal))]
	internal static void OnQueryUserInfoByDisplayNameCallbackInternalImplementation(ref QueryUserInfoByDisplayNameCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryUserInfoByDisplayNameCallbackInfoInternal, OnQueryUserInfoByDisplayNameCallback, QueryUserInfoByDisplayNameCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryUserInfoByExternalAccountCallbackInternal))]
	internal static void OnQueryUserInfoByExternalAccountCallbackInternalImplementation(ref QueryUserInfoByExternalAccountCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryUserInfoByExternalAccountCallbackInfoInternal, OnQueryUserInfoByExternalAccountCallback, QueryUserInfoByExternalAccountCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryUserInfoCallbackInternal))]
	internal static void OnQueryUserInfoCallbackInternalImplementation(ref QueryUserInfoCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryUserInfoCallbackInfoInternal, OnQueryUserInfoCallback, QueryUserInfoCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
