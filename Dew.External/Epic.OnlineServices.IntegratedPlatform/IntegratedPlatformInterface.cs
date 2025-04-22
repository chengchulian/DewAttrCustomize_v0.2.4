using System;

namespace Epic.OnlineServices.IntegratedPlatform;

public sealed class IntegratedPlatformInterface : Handle
{
	public const int AddnotifyuserloginstatuschangedApiLatest = 1;

	public const int ClearuserprelogoutcallbackApiLatest = 1;

	public const int CreateintegratedplatformoptionscontainerApiLatest = 1;

	public const int FinalizedeferreduserlogoutApiLatest = 1;

	public static readonly Utf8String IptSteam = "STEAM";

	public const int OptionsApiLatest = 1;

	public const int SetuserloginstatusApiLatest = 1;

	public const int SetuserprelogoutcallbackApiLatest = 1;

	public const int SteamOptionsApiLatest = 2;

	public IntegratedPlatformInterface()
	{
	}

	public IntegratedPlatformInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyUserLoginStatusChanged(ref AddNotifyUserLoginStatusChangedOptions options, object clientData, OnUserLoginStatusChangedCallback callbackFunction)
	{
		AddNotifyUserLoginStatusChangedOptionsInternal optionsInternal = default(AddNotifyUserLoginStatusChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUserLoginStatusChangedCallbackInternal callbackFunctionInternal = OnUserLoginStatusChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, callbackFunction, callbackFunctionInternal);
		ulong funcResult = Bindings.EOS_IntegratedPlatform_AddNotifyUserLoginStatusChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, callbackFunctionInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public void ClearUserPreLogoutCallback(ref ClearUserPreLogoutCallbackOptions options)
	{
		ClearUserPreLogoutCallbackOptionsInternal optionsInternal = default(ClearUserPreLogoutCallbackOptionsInternal);
		optionsInternal.Set(ref options);
		Bindings.EOS_IntegratedPlatform_ClearUserPreLogoutCallback(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public static Result CreateIntegratedPlatformOptionsContainer(ref CreateIntegratedPlatformOptionsContainerOptions options, out IntegratedPlatformOptionsContainer outIntegratedPlatformOptionsContainerHandle)
	{
		CreateIntegratedPlatformOptionsContainerOptionsInternal optionsInternal = default(CreateIntegratedPlatformOptionsContainerOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outIntegratedPlatformOptionsContainerHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_IntegratedPlatform_CreateIntegratedPlatformOptionsContainer(ref optionsInternal, ref outIntegratedPlatformOptionsContainerHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outIntegratedPlatformOptionsContainerHandleAddress, out outIntegratedPlatformOptionsContainerHandle);
		return result;
	}

	public Result FinalizeDeferredUserLogout(ref FinalizeDeferredUserLogoutOptions options)
	{
		FinalizeDeferredUserLogoutOptionsInternal optionsInternal = default(FinalizeDeferredUserLogoutOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_IntegratedPlatform_FinalizeDeferredUserLogout(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void RemoveNotifyUserLoginStatusChanged(ulong notificationId)
	{
		Bindings.EOS_IntegratedPlatform_RemoveNotifyUserLoginStatusChanged(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public Result SetUserLoginStatus(ref SetUserLoginStatusOptions options)
	{
		SetUserLoginStatusOptionsInternal optionsInternal = default(SetUserLoginStatusOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_IntegratedPlatform_SetUserLoginStatus(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetUserPreLogoutCallback(ref SetUserPreLogoutCallbackOptions options, object clientData, OnUserPreLogoutCallback callbackFunction)
	{
		SetUserPreLogoutCallbackOptionsInternal optionsInternal = default(SetUserPreLogoutCallbackOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUserPreLogoutCallbackInternal callbackFunctionInternal = OnUserPreLogoutCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, callbackFunction, callbackFunctionInternal);
		Result result = Bindings.EOS_IntegratedPlatform_SetUserPreLogoutCallback(base.InnerHandle, ref optionsInternal, clientDataAddress, callbackFunctionInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	[MonoPInvokeCallback(typeof(OnUserLoginStatusChangedCallbackInternal))]
	internal static void OnUserLoginStatusChangedCallbackInternalImplementation(ref UserLoginStatusChangedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<UserLoginStatusChangedCallbackInfoInternal, OnUserLoginStatusChangedCallback, UserLoginStatusChangedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUserPreLogoutCallbackInternal))]
	internal static IntegratedPlatformPreLogoutAction OnUserPreLogoutCallbackInternalImplementation(ref UserPreLogoutCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<UserPreLogoutCallbackInfoInternal, OnUserPreLogoutCallback, UserPreLogoutCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			return callback(ref callbackInfo);
		}
		return Helper.GetDefault<IntegratedPlatformPreLogoutAction>();
	}
}
