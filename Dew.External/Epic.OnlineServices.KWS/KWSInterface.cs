using System;

namespace Epic.OnlineServices.KWS;

public sealed class KWSInterface : Handle
{
	public const int AddnotifypermissionsupdatereceivedApiLatest = 1;

	public const int CopypermissionbyindexApiLatest = 1;

	public const int CreateuserApiLatest = 1;

	public const int GetpermissionbykeyApiLatest = 1;

	public const int GetpermissionscountApiLatest = 1;

	public const int MaxPermissionLength = 32;

	public const int MaxPermissions = 16;

	public const int PermissionstatusApiLatest = 1;

	public const int QueryagegateApiLatest = 1;

	public const int QuerypermissionsApiLatest = 1;

	public const int RequestpermissionsApiLatest = 1;

	public const int UpdateparentemailApiLatest = 1;

	public KWSInterface()
	{
	}

	public KWSInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyPermissionsUpdateReceived(ref AddNotifyPermissionsUpdateReceivedOptions options, object clientData, OnPermissionsUpdateReceivedCallback notificationFn)
	{
		AddNotifyPermissionsUpdateReceivedOptionsInternal optionsInternal = default(AddNotifyPermissionsUpdateReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnPermissionsUpdateReceivedCallbackInternal notificationFnInternal = OnPermissionsUpdateReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_KWS_AddNotifyPermissionsUpdateReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result CopyPermissionByIndex(ref CopyPermissionByIndexOptions options, out PermissionStatus? outPermission)
	{
		CopyPermissionByIndexOptionsInternal optionsInternal = default(CopyPermissionByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outPermissionAddress = IntPtr.Zero;
		Result result = Bindings.EOS_KWS_CopyPermissionByIndex(base.InnerHandle, ref optionsInternal, ref outPermissionAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<PermissionStatusInternal, PermissionStatus>(outPermissionAddress, out outPermission);
		if (outPermission.HasValue)
		{
			Bindings.EOS_KWS_PermissionStatus_Release(outPermissionAddress);
		}
		return result;
	}

	public void CreateUser(ref CreateUserOptions options, object clientData, OnCreateUserCallback completionDelegate)
	{
		CreateUserOptionsInternal optionsInternal = default(CreateUserOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnCreateUserCallbackInternal completionDelegateInternal = OnCreateUserCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_KWS_CreateUser(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result GetPermissionByKey(ref GetPermissionByKeyOptions options, out KWSPermissionStatus outPermission)
	{
		GetPermissionByKeyOptionsInternal optionsInternal = default(GetPermissionByKeyOptionsInternal);
		optionsInternal.Set(ref options);
		outPermission = Helper.GetDefault<KWSPermissionStatus>();
		Result result = Bindings.EOS_KWS_GetPermissionByKey(base.InnerHandle, ref optionsInternal, ref outPermission);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public int GetPermissionsCount(ref GetPermissionsCountOptions options)
	{
		GetPermissionsCountOptionsInternal optionsInternal = default(GetPermissionsCountOptionsInternal);
		optionsInternal.Set(ref options);
		int result = Bindings.EOS_KWS_GetPermissionsCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryAgeGate(ref QueryAgeGateOptions options, object clientData, OnQueryAgeGateCallback completionDelegate)
	{
		QueryAgeGateOptionsInternal optionsInternal = default(QueryAgeGateOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryAgeGateCallbackInternal completionDelegateInternal = OnQueryAgeGateCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_KWS_QueryAgeGate(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryPermissions(ref QueryPermissionsOptions options, object clientData, OnQueryPermissionsCallback completionDelegate)
	{
		QueryPermissionsOptionsInternal optionsInternal = default(QueryPermissionsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryPermissionsCallbackInternal completionDelegateInternal = OnQueryPermissionsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_KWS_QueryPermissions(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyPermissionsUpdateReceived(ulong inId)
	{
		Bindings.EOS_KWS_RemoveNotifyPermissionsUpdateReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RequestPermissions(ref RequestPermissionsOptions options, object clientData, OnRequestPermissionsCallback completionDelegate)
	{
		RequestPermissionsOptionsInternal optionsInternal = default(RequestPermissionsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRequestPermissionsCallbackInternal completionDelegateInternal = OnRequestPermissionsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_KWS_RequestPermissions(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateParentEmail(ref UpdateParentEmailOptions options, object clientData, OnUpdateParentEmailCallback completionDelegate)
	{
		UpdateParentEmailOptionsInternal optionsInternal = default(UpdateParentEmailOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateParentEmailCallbackInternal completionDelegateInternal = OnUpdateParentEmailCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_KWS_UpdateParentEmail(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnCreateUserCallbackInternal))]
	internal static void OnCreateUserCallbackInternalImplementation(ref CreateUserCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<CreateUserCallbackInfoInternal, OnCreateUserCallback, CreateUserCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnPermissionsUpdateReceivedCallbackInternal))]
	internal static void OnPermissionsUpdateReceivedCallbackInternalImplementation(ref PermissionsUpdateReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<PermissionsUpdateReceivedCallbackInfoInternal, OnPermissionsUpdateReceivedCallback, PermissionsUpdateReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryAgeGateCallbackInternal))]
	internal static void OnQueryAgeGateCallbackInternalImplementation(ref QueryAgeGateCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryAgeGateCallbackInfoInternal, OnQueryAgeGateCallback, QueryAgeGateCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryPermissionsCallbackInternal))]
	internal static void OnQueryPermissionsCallbackInternalImplementation(ref QueryPermissionsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryPermissionsCallbackInfoInternal, OnQueryPermissionsCallback, QueryPermissionsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRequestPermissionsCallbackInternal))]
	internal static void OnRequestPermissionsCallbackInternalImplementation(ref RequestPermissionsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<RequestPermissionsCallbackInfoInternal, OnRequestPermissionsCallback, RequestPermissionsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateParentEmailCallbackInternal))]
	internal static void OnUpdateParentEmailCallbackInternalImplementation(ref UpdateParentEmailCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateParentEmailCallbackInfoInternal, OnUpdateParentEmailCallback, UpdateParentEmailCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
