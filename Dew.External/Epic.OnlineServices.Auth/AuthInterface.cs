using System;

namespace Epic.OnlineServices.Auth;

public sealed class AuthInterface : Handle
{
	public const int AccountfeaturerestrictedinfoApiLatest = 1;

	public const int AddnotifyloginstatuschangedApiLatest = 1;

	public const int CopyidtokenApiLatest = 1;

	public const int CopyuserauthtokenApiLatest = 1;

	public const int CredentialsApiLatest = 4;

	public const int DeletepersistentauthApiLatest = 2;

	public const int IdtokenApiLatest = 1;

	public const int LinkaccountApiLatest = 1;

	public const int LoginApiLatest = 3;

	public const int LogoutApiLatest = 1;

	public const int PingrantinfoApiLatest = 2;

	public const int QueryidtokenApiLatest = 1;

	public const int TokenApiLatest = 2;

	public const int VerifyidtokenApiLatest = 1;

	public const int VerifyuserauthApiLatest = 1;

	public const int IosCredentialssystemauthcredentialsoptionsApiLatest = 1;

	public AuthInterface()
	{
	}

	public AuthInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyLoginStatusChanged(ref AddNotifyLoginStatusChangedOptions options, object clientData, OnLoginStatusChangedCallback notification)
	{
		AddNotifyLoginStatusChangedOptionsInternal optionsInternal = default(AddNotifyLoginStatusChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLoginStatusChangedCallbackInternal notificationInternal = OnLoginStatusChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notification, notificationInternal);
		ulong funcResult = Bindings.EOS_Auth_AddNotifyLoginStatusChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result CopyIdToken(ref CopyIdTokenOptions options, out IdToken? outIdToken)
	{
		CopyIdTokenOptionsInternal optionsInternal = default(CopyIdTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outIdTokenAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Auth_CopyIdToken(base.InnerHandle, ref optionsInternal, ref outIdTokenAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<IdTokenInternal, IdToken>(outIdTokenAddress, out outIdToken);
		if (outIdToken.HasValue)
		{
			Bindings.EOS_Auth_IdToken_Release(outIdTokenAddress);
		}
		return result;
	}

	public Result CopyUserAuthToken(ref CopyUserAuthTokenOptions options, EpicAccountId localUserId, out Token? outUserAuthToken)
	{
		CopyUserAuthTokenOptionsInternal optionsInternal = default(CopyUserAuthTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr localUserIdInnerHandle = IntPtr.Zero;
		Helper.Set(localUserId, ref localUserIdInnerHandle);
		IntPtr outUserAuthTokenAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Auth_CopyUserAuthToken(base.InnerHandle, ref optionsInternal, localUserIdInnerHandle, ref outUserAuthTokenAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<TokenInternal, Token>(outUserAuthTokenAddress, out outUserAuthToken);
		if (outUserAuthToken.HasValue)
		{
			Bindings.EOS_Auth_Token_Release(outUserAuthTokenAddress);
		}
		return result;
	}

	public void DeletePersistentAuth(ref DeletePersistentAuthOptions options, object clientData, OnDeletePersistentAuthCallback completionDelegate)
	{
		DeletePersistentAuthOptionsInternal optionsInternal = default(DeletePersistentAuthOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDeletePersistentAuthCallbackInternal completionDelegateInternal = OnDeletePersistentAuthCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Auth_DeletePersistentAuth(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public EpicAccountId GetLoggedInAccountByIndex(int index)
	{
		Helper.Get(Bindings.EOS_Auth_GetLoggedInAccountByIndex(base.InnerHandle, index), out EpicAccountId funcResultReturn);
		return funcResultReturn;
	}

	public int GetLoggedInAccountsCount()
	{
		return Bindings.EOS_Auth_GetLoggedInAccountsCount(base.InnerHandle);
	}

	public LoginStatus GetLoginStatus(EpicAccountId localUserId)
	{
		IntPtr localUserIdInnerHandle = IntPtr.Zero;
		Helper.Set(localUserId, ref localUserIdInnerHandle);
		return Bindings.EOS_Auth_GetLoginStatus(base.InnerHandle, localUserIdInnerHandle);
	}

	public EpicAccountId GetMergedAccountByIndex(EpicAccountId localUserId, uint index)
	{
		IntPtr localUserIdInnerHandle = IntPtr.Zero;
		Helper.Set(localUserId, ref localUserIdInnerHandle);
		Helper.Get(Bindings.EOS_Auth_GetMergedAccountByIndex(base.InnerHandle, localUserIdInnerHandle, index), out EpicAccountId funcResultReturn);
		return funcResultReturn;
	}

	public uint GetMergedAccountsCount(EpicAccountId localUserId)
	{
		IntPtr localUserIdInnerHandle = IntPtr.Zero;
		Helper.Set(localUserId, ref localUserIdInnerHandle);
		return Bindings.EOS_Auth_GetMergedAccountsCount(base.InnerHandle, localUserIdInnerHandle);
	}

	public Result GetSelectedAccountId(EpicAccountId localUserId, out EpicAccountId outSelectedAccountId)
	{
		IntPtr localUserIdInnerHandle = IntPtr.Zero;
		Helper.Set(localUserId, ref localUserIdInnerHandle);
		IntPtr outSelectedAccountIdAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Auth_GetSelectedAccountId(base.InnerHandle, localUserIdInnerHandle, ref outSelectedAccountIdAddress);
		Helper.Get(outSelectedAccountIdAddress, out outSelectedAccountId);
		return result;
	}

	public void LinkAccount(ref LinkAccountOptions options, object clientData, OnLinkAccountCallback completionDelegate)
	{
		LinkAccountOptionsInternal optionsInternal = default(LinkAccountOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLinkAccountCallbackInternal completionDelegateInternal = OnLinkAccountCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Auth_LinkAccount(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void Login(ref LoginOptions options, object clientData, OnLoginCallback completionDelegate)
	{
		LoginOptionsInternal optionsInternal = default(LoginOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLoginCallbackInternal completionDelegateInternal = OnLoginCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Auth_Login(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void Logout(ref LogoutOptions options, object clientData, OnLogoutCallback completionDelegate)
	{
		LogoutOptionsInternal optionsInternal = default(LogoutOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLogoutCallbackInternal completionDelegateInternal = OnLogoutCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Auth_Logout(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryIdToken(ref QueryIdTokenOptions options, object clientData, OnQueryIdTokenCallback completionDelegate)
	{
		QueryIdTokenOptionsInternal optionsInternal = default(QueryIdTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryIdTokenCallbackInternal completionDelegateInternal = OnQueryIdTokenCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Auth_QueryIdToken(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyLoginStatusChanged(ulong inId)
	{
		Bindings.EOS_Auth_RemoveNotifyLoginStatusChanged(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void VerifyIdToken(ref VerifyIdTokenOptions options, object clientData, OnVerifyIdTokenCallback completionDelegate)
	{
		VerifyIdTokenOptionsInternal optionsInternal = default(VerifyIdTokenOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnVerifyIdTokenCallbackInternal completionDelegateInternal = OnVerifyIdTokenCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Auth_VerifyIdToken(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void VerifyUserAuth(ref VerifyUserAuthOptions options, object clientData, OnVerifyUserAuthCallback completionDelegate)
	{
		VerifyUserAuthOptionsInternal optionsInternal = default(VerifyUserAuthOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnVerifyUserAuthCallbackInternal completionDelegateInternal = OnVerifyUserAuthCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Auth_VerifyUserAuth(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnDeletePersistentAuthCallbackInternal))]
	internal static void OnDeletePersistentAuthCallbackInternalImplementation(ref DeletePersistentAuthCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<DeletePersistentAuthCallbackInfoInternal, OnDeletePersistentAuthCallback, DeletePersistentAuthCallbackInfo>(ref data, out var callback, out var callbackInfo))
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

	[MonoPInvokeCallback(typeof(OnLogoutCallbackInternal))]
	internal static void OnLogoutCallbackInternalImplementation(ref LogoutCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<LogoutCallbackInfoInternal, OnLogoutCallback, LogoutCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryIdTokenCallbackInternal))]
	internal static void OnQueryIdTokenCallbackInternalImplementation(ref QueryIdTokenCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryIdTokenCallbackInfoInternal, OnQueryIdTokenCallback, QueryIdTokenCallbackInfo>(ref data, out var callback, out var callbackInfo))
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

	[MonoPInvokeCallback(typeof(OnVerifyUserAuthCallbackInternal))]
	internal static void OnVerifyUserAuthCallbackInternalImplementation(ref VerifyUserAuthCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<VerifyUserAuthCallbackInfoInternal, OnVerifyUserAuthCallback, VerifyUserAuthCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	public void Login(ref IOSLoginOptions options, object clientData, OnLoginCallback completionDelegate)
	{
		IOSLoginOptionsInternal optionsInternal = default(IOSLoginOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLoginCallbackInternal completionDelegateInternal = OnLoginCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		IOSBindings.EOS_Auth_Login(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}
}
