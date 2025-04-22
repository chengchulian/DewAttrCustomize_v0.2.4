using System;

namespace Epic.OnlineServices.Connect;

public sealed class ConnectInterface : Handle
{
	public const int AddnotifyauthexpirationApiLatest = 1;

	public const int AddnotifyloginstatuschangedApiLatest = 1;

	public const int CopyidtokenApiLatest = 1;

	public const int CopyproductuserexternalaccountbyaccountidApiLatest = 1;

	public const int CopyproductuserexternalaccountbyaccounttypeApiLatest = 1;

	public const int CopyproductuserexternalaccountbyindexApiLatest = 1;

	public const int CopyproductuserinfoApiLatest = 1;

	public const int CreatedeviceidApiLatest = 1;

	public const int CreatedeviceidDevicemodelMaxLength = 64;

	public const int CreateuserApiLatest = 1;

	public const int CredentialsApiLatest = 1;

	public const int DeletedeviceidApiLatest = 1;

	public const int ExternalAccountIdMaxLength = 256;

	public const int ExternalaccountinfoApiLatest = 1;

	public const int GetexternalaccountmappingApiLatest = 1;

	public const int GetexternalaccountmappingsApiLatest = 1;

	public const int GetproductuserexternalaccountcountApiLatest = 1;

	public const int GetproductuseridmappingApiLatest = 1;

	public const int IdtokenApiLatest = 1;

	public const int LinkaccountApiLatest = 1;

	public const int LoginApiLatest = 2;

	public const int OnauthexpirationcallbackApiLatest = 1;

	public const int QueryexternalaccountmappingsApiLatest = 1;

	public const int QueryexternalaccountmappingsMaxAccountIds = 128;

	public const int QueryproductuseridmappingsApiLatest = 2;

	public const int TimeUndefined = -1;

	public const int TransferdeviceidaccountApiLatest = 1;

	public const int UnlinkaccountApiLatest = 1;

	public const int UserlogininfoApiLatest = 2;

	public const int UserlogininfoDisplaynameMaxLength = 32;

	public const int VerifyidtokenApiLatest = 1;

	public ConnectInterface()
	{
	}

	public ConnectInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyAuthExpiration(ref AddNotifyAuthExpirationOptions options, object clientData, OnAuthExpirationCallback notification)
	{
		AddNotifyAuthExpirationOptionsInternal optionsInternal = default(AddNotifyAuthExpirationOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAuthExpirationCallbackInternal notificationInternal = OnAuthExpirationCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notification, notificationInternal);
		ulong funcResult = Bindings.EOS_Connect_AddNotifyAuthExpiration(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLoginStatusChanged(ref AddNotifyLoginStatusChangedOptions options, object clientData, OnLoginStatusChangedCallback notification)
	{
		AddNotifyLoginStatusChangedOptionsInternal optionsInternal = default(AddNotifyLoginStatusChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLoginStatusChangedCallbackInternal notificationInternal = OnLoginStatusChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notification, notificationInternal);
		ulong funcResult = Bindings.EOS_Connect_AddNotifyLoginStatusChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result CopyIdToken(ref CopyIdTokenOptions options, out IdToken? outIdToken)
	{
		CopyIdTokenOptionsInternal optionsInternal = default(CopyIdTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outIdTokenAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Connect_CopyIdToken(base.InnerHandle, ref optionsInternal, ref outIdTokenAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<IdTokenInternal, IdToken>(outIdTokenAddress, out outIdToken);
		if (outIdToken.HasValue)
		{
			Bindings.EOS_Connect_IdToken_Release(outIdTokenAddress);
		}
		return result;
	}

	public Result CopyProductUserExternalAccountByAccountId(ref CopyProductUserExternalAccountByAccountIdOptions options, out ExternalAccountInfo? outExternalAccountInfo)
	{
		CopyProductUserExternalAccountByAccountIdOptionsInternal optionsInternal = default(CopyProductUserExternalAccountByAccountIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outExternalAccountInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Connect_CopyProductUserExternalAccountByAccountId(base.InnerHandle, ref optionsInternal, ref outExternalAccountInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ExternalAccountInfoInternal, ExternalAccountInfo>(outExternalAccountInfoAddress, out outExternalAccountInfo);
		if (outExternalAccountInfo.HasValue)
		{
			Bindings.EOS_Connect_ExternalAccountInfo_Release(outExternalAccountInfoAddress);
		}
		return result;
	}

	public Result CopyProductUserExternalAccountByAccountType(ref CopyProductUserExternalAccountByAccountTypeOptions options, out ExternalAccountInfo? outExternalAccountInfo)
	{
		CopyProductUserExternalAccountByAccountTypeOptionsInternal optionsInternal = default(CopyProductUserExternalAccountByAccountTypeOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outExternalAccountInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Connect_CopyProductUserExternalAccountByAccountType(base.InnerHandle, ref optionsInternal, ref outExternalAccountInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ExternalAccountInfoInternal, ExternalAccountInfo>(outExternalAccountInfoAddress, out outExternalAccountInfo);
		if (outExternalAccountInfo.HasValue)
		{
			Bindings.EOS_Connect_ExternalAccountInfo_Release(outExternalAccountInfoAddress);
		}
		return result;
	}

	public Result CopyProductUserExternalAccountByIndex(ref CopyProductUserExternalAccountByIndexOptions options, out ExternalAccountInfo? outExternalAccountInfo)
	{
		CopyProductUserExternalAccountByIndexOptionsInternal optionsInternal = default(CopyProductUserExternalAccountByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outExternalAccountInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Connect_CopyProductUserExternalAccountByIndex(base.InnerHandle, ref optionsInternal, ref outExternalAccountInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ExternalAccountInfoInternal, ExternalAccountInfo>(outExternalAccountInfoAddress, out outExternalAccountInfo);
		if (outExternalAccountInfo.HasValue)
		{
			Bindings.EOS_Connect_ExternalAccountInfo_Release(outExternalAccountInfoAddress);
		}
		return result;
	}

	public Result CopyProductUserInfo(ref CopyProductUserInfoOptions options, out ExternalAccountInfo? outExternalAccountInfo)
	{
		CopyProductUserInfoOptionsInternal optionsInternal = default(CopyProductUserInfoOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outExternalAccountInfoAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Connect_CopyProductUserInfo(base.InnerHandle, ref optionsInternal, ref outExternalAccountInfoAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<ExternalAccountInfoInternal, ExternalAccountInfo>(outExternalAccountInfoAddress, out outExternalAccountInfo);
		if (outExternalAccountInfo.HasValue)
		{
			Bindings.EOS_Connect_ExternalAccountInfo_Release(outExternalAccountInfoAddress);
		}
		return result;
	}

	public void CreateDeviceId(ref CreateDeviceIdOptions options, object clientData, OnCreateDeviceIdCallback completionDelegate)
	{
		CreateDeviceIdOptionsInternal optionsInternal = default(CreateDeviceIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnCreateDeviceIdCallbackInternal completionDelegateInternal = OnCreateDeviceIdCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_CreateDeviceId(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void CreateUser(ref CreateUserOptions options, object clientData, OnCreateUserCallback completionDelegate)
	{
		CreateUserOptionsInternal optionsInternal = default(CreateUserOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnCreateUserCallbackInternal completionDelegateInternal = OnCreateUserCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_CreateUser(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void DeleteDeviceId(ref DeleteDeviceIdOptions options, object clientData, OnDeleteDeviceIdCallback completionDelegate)
	{
		DeleteDeviceIdOptionsInternal optionsInternal = default(DeleteDeviceIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDeleteDeviceIdCallbackInternal completionDelegateInternal = OnDeleteDeviceIdCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_DeleteDeviceId(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public ProductUserId GetExternalAccountMapping(ref GetExternalAccountMappingsOptions options)
	{
		GetExternalAccountMappingsOptionsInternal optionsInternal = default(GetExternalAccountMappingsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_Connect_GetExternalAccountMapping(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out ProductUserId funcResultReturn);
		return funcResultReturn;
	}

	public ProductUserId GetLoggedInUserByIndex(int index)
	{
		Helper.Get(Bindings.EOS_Connect_GetLoggedInUserByIndex(base.InnerHandle, index), out ProductUserId funcResultReturn);
		return funcResultReturn;
	}

	public int GetLoggedInUsersCount()
	{
		return Bindings.EOS_Connect_GetLoggedInUsersCount(base.InnerHandle);
	}

	public LoginStatus GetLoginStatus(ProductUserId localUserId)
	{
		IntPtr localUserIdInnerHandle = IntPtr.Zero;
		Helper.Set(localUserId, ref localUserIdInnerHandle);
		return Bindings.EOS_Connect_GetLoginStatus(base.InnerHandle, localUserIdInnerHandle);
	}

	public uint GetProductUserExternalAccountCount(ref GetProductUserExternalAccountCountOptions options)
	{
		GetProductUserExternalAccountCountOptionsInternal optionsInternal = default(GetProductUserExternalAccountCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Connect_GetProductUserExternalAccountCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetProductUserIdMapping(ref GetProductUserIdMappingOptions options, out Utf8String outBuffer)
	{
		GetProductUserIdMappingOptionsInternal optionsInternal = default(GetProductUserIdMappingOptionsInternal);
		optionsInternal.Set(ref options);
		int inOutBufferLength = 257;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Connect_GetProductUserIdMapping(base.InnerHandle, ref optionsInternal, outBufferAddress, ref inOutBufferLength);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public void LinkAccount(ref LinkAccountOptions options, object clientData, OnLinkAccountCallback completionDelegate)
	{
		LinkAccountOptionsInternal optionsInternal = default(LinkAccountOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLinkAccountCallbackInternal completionDelegateInternal = OnLinkAccountCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_LinkAccount(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void Login(ref LoginOptions options, object clientData, OnLoginCallback completionDelegate)
	{
		LoginOptionsInternal optionsInternal = default(LoginOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLoginCallbackInternal completionDelegateInternal = OnLoginCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_Login(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryExternalAccountMappings(ref QueryExternalAccountMappingsOptions options, object clientData, OnQueryExternalAccountMappingsCallback completionDelegate)
	{
		QueryExternalAccountMappingsOptionsInternal optionsInternal = default(QueryExternalAccountMappingsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryExternalAccountMappingsCallbackInternal completionDelegateInternal = OnQueryExternalAccountMappingsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_QueryExternalAccountMappings(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryProductUserIdMappings(ref QueryProductUserIdMappingsOptions options, object clientData, OnQueryProductUserIdMappingsCallback completionDelegate)
	{
		QueryProductUserIdMappingsOptionsInternal optionsInternal = default(QueryProductUserIdMappingsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryProductUserIdMappingsCallbackInternal completionDelegateInternal = OnQueryProductUserIdMappingsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_QueryProductUserIdMappings(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyAuthExpiration(ulong inId)
	{
		Bindings.EOS_Connect_RemoveNotifyAuthExpiration(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLoginStatusChanged(ulong inId)
	{
		Bindings.EOS_Connect_RemoveNotifyLoginStatusChanged(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void TransferDeviceIdAccount(ref TransferDeviceIdAccountOptions options, object clientData, OnTransferDeviceIdAccountCallback completionDelegate)
	{
		TransferDeviceIdAccountOptionsInternal optionsInternal = default(TransferDeviceIdAccountOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnTransferDeviceIdAccountCallbackInternal completionDelegateInternal = OnTransferDeviceIdAccountCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_TransferDeviceIdAccount(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UnlinkAccount(ref UnlinkAccountOptions options, object clientData, OnUnlinkAccountCallback completionDelegate)
	{
		UnlinkAccountOptionsInternal optionsInternal = default(UnlinkAccountOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUnlinkAccountCallbackInternal completionDelegateInternal = OnUnlinkAccountCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_UnlinkAccount(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void VerifyIdToken(ref VerifyIdTokenOptions options, object clientData, OnVerifyIdTokenCallback completionDelegate)
	{
		VerifyIdTokenOptionsInternal optionsInternal = default(VerifyIdTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnVerifyIdTokenCallbackInternal completionDelegateInternal = OnVerifyIdTokenCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Connect_VerifyIdToken(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnAuthExpirationCallbackInternal))]
	internal static void OnAuthExpirationCallbackInternalImplementation(ref AuthExpirationCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<AuthExpirationCallbackInfoInternal, OnAuthExpirationCallback, AuthExpirationCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnCreateDeviceIdCallbackInternal))]
	internal static void OnCreateDeviceIdCallbackInternalImplementation(ref CreateDeviceIdCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<CreateDeviceIdCallbackInfoInternal, OnCreateDeviceIdCallback, CreateDeviceIdCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnCreateUserCallbackInternal))]
	internal static void OnCreateUserCallbackInternalImplementation(ref CreateUserCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<CreateUserCallbackInfoInternal, OnCreateUserCallback, CreateUserCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnDeleteDeviceIdCallbackInternal))]
	internal static void OnDeleteDeviceIdCallbackInternalImplementation(ref DeleteDeviceIdCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<DeleteDeviceIdCallbackInfoInternal, OnDeleteDeviceIdCallback, DeleteDeviceIdCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLinkAccountCallbackInternal))]
	internal static void OnLinkAccountCallbackInternalImplementation(ref LinkAccountCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<LinkAccountCallbackInfoInternal, OnLinkAccountCallback, LinkAccountCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLoginCallbackInternal))]
	internal static void OnLoginCallbackInternalImplementation(ref LoginCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<LoginCallbackInfoInternal, OnLoginCallback, LoginCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLoginStatusChangedCallbackInternal))]
	internal static void OnLoginStatusChangedCallbackInternalImplementation(ref LoginStatusChangedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LoginStatusChangedCallbackInfoInternal, OnLoginStatusChangedCallback, LoginStatusChangedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryExternalAccountMappingsCallbackInternal))]
	internal static void OnQueryExternalAccountMappingsCallbackInternalImplementation(ref QueryExternalAccountMappingsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryExternalAccountMappingsCallbackInfoInternal, OnQueryExternalAccountMappingsCallback, QueryExternalAccountMappingsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryProductUserIdMappingsCallbackInternal))]
	internal static void OnQueryProductUserIdMappingsCallbackInternalImplementation(ref QueryProductUserIdMappingsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryProductUserIdMappingsCallbackInfoInternal, OnQueryProductUserIdMappingsCallback, QueryProductUserIdMappingsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnTransferDeviceIdAccountCallbackInternal))]
	internal static void OnTransferDeviceIdAccountCallbackInternalImplementation(ref TransferDeviceIdAccountCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<TransferDeviceIdAccountCallbackInfoInternal, OnTransferDeviceIdAccountCallback, TransferDeviceIdAccountCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUnlinkAccountCallbackInternal))]
	internal static void OnUnlinkAccountCallbackInternalImplementation(ref UnlinkAccountCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UnlinkAccountCallbackInfoInternal, OnUnlinkAccountCallback, UnlinkAccountCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnVerifyIdTokenCallbackInternal))]
	internal static void OnVerifyIdTokenCallbackInternalImplementation(ref VerifyIdTokenCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<VerifyIdTokenCallbackInfoInternal, OnVerifyIdTokenCallback, VerifyIdTokenCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
