using System;

namespace Epic.OnlineServices.Presence;

public sealed class PresenceInterface : Handle
{
	public const int AddnotifyjoingameacceptedApiLatest = 2;

	public const int AddnotifyonpresencechangedApiLatest = 1;

	public const int CopypresenceApiLatest = 3;

	public const int CreatepresencemodificationApiLatest = 1;

	public const int DataMaxKeyLength = 64;

	public const int DataMaxKeys = 32;

	public const int DataMaxValueLength = 255;

	public const int DatarecordApiLatest = 1;

	public const int DeletedataApiLatest = 1;

	public const int GetjoininfoApiLatest = 1;

	public const int HaspresenceApiLatest = 1;

	public const int InfoApiLatest = 3;

	public static readonly Utf8String KeyPlatformPresence = "EOS_PlatformPresence";

	public const int QuerypresenceApiLatest = 1;

	public const int RichTextMaxValueLength = 255;

	public const int SetdataApiLatest = 1;

	public const int SetpresenceApiLatest = 1;

	public const int SetrawrichtextApiLatest = 1;

	public const int SetstatusApiLatest = 1;

	public PresenceInterface()
	{
	}

	public PresenceInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyJoinGameAccepted(ref AddNotifyJoinGameAcceptedOptions options, object clientData, OnJoinGameAcceptedCallback notificationFn)
	{
		AddNotifyJoinGameAcceptedOptionsInternal optionsInternal = default(AddNotifyJoinGameAcceptedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnJoinGameAcceptedCallbackInternal notificationFnInternal = OnJoinGameAcceptedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Presence_AddNotifyJoinGameAccepted(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyOnPresenceChanged(ref AddNotifyOnPresenceChangedOptions options, object clientData, OnPresenceChangedCallback notificationHandler)
	{
		AddNotifyOnPresenceChangedOptionsInternal optionsInternal = default(AddNotifyOnPresenceChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnPresenceChangedCallbackInternal notificationHandlerInternal = OnPresenceChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationHandler, notificationHandlerInternal);
		ulong funcResult = Bindings.EOS_Presence_AddNotifyOnPresenceChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result CopyPresence(ref CopyPresenceOptions options, out Info? outPresence)
	{
		CopyPresenceOptionsInternal optionsInternal = default(CopyPresenceOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outPresenceAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Presence_CopyPresence(base.InnerHandle, ref optionsInternal, ref outPresenceAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<InfoInternal, Info>(outPresenceAddress, out outPresence);
		if (outPresence.HasValue)
		{
			Bindings.EOS_Presence_Info_Release(outPresenceAddress);
		}
		return result;
	}

	public Result CreatePresenceModification(ref CreatePresenceModificationOptions options, out PresenceModification outPresenceModificationHandle)
	{
		CreatePresenceModificationOptionsInternal optionsInternal = default(CreatePresenceModificationOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outPresenceModificationHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Presence_CreatePresenceModification(base.InnerHandle, ref optionsInternal, ref outPresenceModificationHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outPresenceModificationHandleAddress, out outPresenceModificationHandle);
		return result;
	}

	public Result GetJoinInfo(ref GetJoinInfoOptions options, out Utf8String outBuffer)
	{
		GetJoinInfoOptionsInternal optionsInternal = default(GetJoinInfoOptionsInternal);
		optionsInternal.Set(ref options);
		int inOutBufferLength = 256;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Presence_GetJoinInfo(base.InnerHandle, ref optionsInternal, outBufferAddress, ref inOutBufferLength);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public bool HasPresence(ref HasPresenceOptions options)
	{
		HasPresenceOptionsInternal optionsInternal = default(HasPresenceOptionsInternal);
		optionsInternal.Set(ref options);
		int from = Bindings.EOS_Presence_HasPresence(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out var funcResultReturn);
		return funcResultReturn;
	}

	public void QueryPresence(ref QueryPresenceOptions options, object clientData, OnQueryPresenceCompleteCallback completionDelegate)
	{
		QueryPresenceOptionsInternal optionsInternal = default(QueryPresenceOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryPresenceCompleteCallbackInternal completionDelegateInternal = OnQueryPresenceCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Presence_QueryPresence(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyJoinGameAccepted(ulong inId)
	{
		Bindings.EOS_Presence_RemoveNotifyJoinGameAccepted(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyOnPresenceChanged(ulong notificationId)
	{
		Bindings.EOS_Presence_RemoveNotifyOnPresenceChanged(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void SetPresence(ref SetPresenceOptions options, object clientData, SetPresenceCompleteCallback completionDelegate)
	{
		SetPresenceOptionsInternal optionsInternal = default(SetPresenceOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		SetPresenceCompleteCallbackInternal completionDelegateInternal = SetPresenceCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Presence_SetPresence(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnJoinGameAcceptedCallbackInternal))]
	internal static void OnJoinGameAcceptedCallbackInternalImplementation(ref JoinGameAcceptedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<JoinGameAcceptedCallbackInfoInternal, OnJoinGameAcceptedCallback, JoinGameAcceptedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnPresenceChangedCallbackInternal))]
	internal static void OnPresenceChangedCallbackInternalImplementation(ref PresenceChangedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<PresenceChangedCallbackInfoInternal, OnPresenceChangedCallback, PresenceChangedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryPresenceCompleteCallbackInternal))]
	internal static void OnQueryPresenceCompleteCallbackInternalImplementation(ref QueryPresenceCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryPresenceCallbackInfoInternal, OnQueryPresenceCompleteCallback, QueryPresenceCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(SetPresenceCompleteCallbackInternal))]
	internal static void SetPresenceCompleteCallbackInternalImplementation(ref SetPresenceCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<SetPresenceCallbackInfoInternal, SetPresenceCompleteCallback, SetPresenceCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
