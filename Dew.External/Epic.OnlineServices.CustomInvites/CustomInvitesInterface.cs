using System;

namespace Epic.OnlineServices.CustomInvites;

public sealed class CustomInvitesInterface : Handle
{
	public const int AcceptrequesttojoinApiLatest = 1;

	public const int AddnotifycustominviteacceptedApiLatest = 1;

	public const int AddnotifycustominvitereceivedApiLatest = 1;

	public const int AddnotifycustominviterejectedApiLatest = 1;

	public const int AddnotifyrequesttojoinacceptedApiLatest = 1;

	public const int AddnotifyrequesttojoinreceivedApiLatest = 1;

	public const int AddnotifyrequesttojoinrejectedApiLatest = 1;

	public const int AddnotifyrequesttojoinresponsereceivedApiLatest = 1;

	public const int AddnotifysendcustomnativeinviterequestedApiLatest = 1;

	public const int FinalizeinviteApiLatest = 1;

	public const int MaxPayloadLength = 500;

	public const int RejectrequesttojoinApiLatest = 1;

	public const int SendcustominviteApiLatest = 1;

	public const int SendrequesttojoinApiLatest = 1;

	public const int SetcustominviteApiLatest = 1;

	public CustomInvitesInterface()
	{
	}

	public CustomInvitesInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public void AcceptRequestToJoin(ref AcceptRequestToJoinOptions options, object clientData, OnAcceptRequestToJoinCallback completionDelegate)
	{
		AcceptRequestToJoinOptionsInternal optionsInternal = default(AcceptRequestToJoinOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAcceptRequestToJoinCallbackInternal completionDelegateInternal = OnAcceptRequestToJoinCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_CustomInvites_AcceptRequestToJoin(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public ulong AddNotifyCustomInviteAccepted(ref AddNotifyCustomInviteAcceptedOptions options, object clientData, OnCustomInviteAcceptedCallback notificationFn)
	{
		AddNotifyCustomInviteAcceptedOptionsInternal optionsInternal = default(AddNotifyCustomInviteAcceptedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnCustomInviteAcceptedCallbackInternal notificationFnInternal = OnCustomInviteAcceptedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_CustomInvites_AddNotifyCustomInviteAccepted(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyCustomInviteReceived(ref AddNotifyCustomInviteReceivedOptions options, object clientData, OnCustomInviteReceivedCallback notificationFn)
	{
		AddNotifyCustomInviteReceivedOptionsInternal optionsInternal = default(AddNotifyCustomInviteReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnCustomInviteReceivedCallbackInternal notificationFnInternal = OnCustomInviteReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_CustomInvites_AddNotifyCustomInviteReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyCustomInviteRejected(ref AddNotifyCustomInviteRejectedOptions options, object clientData, OnCustomInviteRejectedCallback notificationFn)
	{
		AddNotifyCustomInviteRejectedOptionsInternal optionsInternal = default(AddNotifyCustomInviteRejectedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnCustomInviteRejectedCallbackInternal notificationFnInternal = OnCustomInviteRejectedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_CustomInvites_AddNotifyCustomInviteRejected(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyRequestToJoinAccepted(ref AddNotifyRequestToJoinAcceptedOptions options, object clientData, OnRequestToJoinAcceptedCallback notificationFn)
	{
		AddNotifyRequestToJoinAcceptedOptionsInternal optionsInternal = default(AddNotifyRequestToJoinAcceptedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRequestToJoinAcceptedCallbackInternal notificationFnInternal = OnRequestToJoinAcceptedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_CustomInvites_AddNotifyRequestToJoinAccepted(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyRequestToJoinReceived(ref AddNotifyRequestToJoinReceivedOptions options, object clientData, OnRequestToJoinReceivedCallback notificationFn)
	{
		AddNotifyRequestToJoinReceivedOptionsInternal optionsInternal = default(AddNotifyRequestToJoinReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRequestToJoinReceivedCallbackInternal notificationFnInternal = OnRequestToJoinReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_CustomInvites_AddNotifyRequestToJoinReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyRequestToJoinRejected(ref AddNotifyRequestToJoinRejectedOptions options, object clientData, OnRequestToJoinRejectedCallback notificationFn)
	{
		AddNotifyRequestToJoinRejectedOptionsInternal optionsInternal = default(AddNotifyRequestToJoinRejectedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRequestToJoinRejectedCallbackInternal notificationFnInternal = OnRequestToJoinRejectedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_CustomInvites_AddNotifyRequestToJoinRejected(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyRequestToJoinResponseReceived(ref AddNotifyRequestToJoinResponseReceivedOptions options, object clientData, OnRequestToJoinResponseReceivedCallback notificationFn)
	{
		AddNotifyRequestToJoinResponseReceivedOptionsInternal optionsInternal = default(AddNotifyRequestToJoinResponseReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRequestToJoinResponseReceivedCallbackInternal notificationFnInternal = OnRequestToJoinResponseReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_CustomInvites_AddNotifyRequestToJoinResponseReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifySendCustomNativeInviteRequested(ref AddNotifySendCustomNativeInviteRequestedOptions options, object clientData, OnSendCustomNativeInviteRequestedCallback notificationFn)
	{
		AddNotifySendCustomNativeInviteRequestedOptionsInternal optionsInternal = default(AddNotifySendCustomNativeInviteRequestedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendCustomNativeInviteRequestedCallbackInternal notificationFnInternal = OnSendCustomNativeInviteRequestedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_CustomInvites_AddNotifySendCustomNativeInviteRequested(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result FinalizeInvite(ref FinalizeInviteOptions options)
	{
		FinalizeInviteOptionsInternal optionsInternal = default(FinalizeInviteOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_CustomInvites_FinalizeInvite(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void RejectRequestToJoin(ref RejectRequestToJoinOptions options, object clientData, OnRejectRequestToJoinCallback completionDelegate)
	{
		RejectRequestToJoinOptionsInternal optionsInternal = default(RejectRequestToJoinOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRejectRequestToJoinCallbackInternal completionDelegateInternal = OnRejectRequestToJoinCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_CustomInvites_RejectRequestToJoin(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyCustomInviteAccepted(ulong inId)
	{
		Bindings.EOS_CustomInvites_RemoveNotifyCustomInviteAccepted(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyCustomInviteReceived(ulong inId)
	{
		Bindings.EOS_CustomInvites_RemoveNotifyCustomInviteReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyCustomInviteRejected(ulong inId)
	{
		Bindings.EOS_CustomInvites_RemoveNotifyCustomInviteRejected(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyRequestToJoinAccepted(ulong inId)
	{
		Bindings.EOS_CustomInvites_RemoveNotifyRequestToJoinAccepted(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyRequestToJoinReceived(ulong inId)
	{
		Bindings.EOS_CustomInvites_RemoveNotifyRequestToJoinReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyRequestToJoinRejected(ulong inId)
	{
		Bindings.EOS_CustomInvites_RemoveNotifyRequestToJoinRejected(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyRequestToJoinResponseReceived(ulong inId)
	{
		Bindings.EOS_CustomInvites_RemoveNotifyRequestToJoinResponseReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifySendCustomNativeInviteRequested(ulong inId)
	{
		Bindings.EOS_CustomInvites_RemoveNotifySendCustomNativeInviteRequested(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void SendCustomInvite(ref SendCustomInviteOptions options, object clientData, OnSendCustomInviteCallback completionDelegate)
	{
		SendCustomInviteOptionsInternal optionsInternal = default(SendCustomInviteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendCustomInviteCallbackInternal completionDelegateInternal = OnSendCustomInviteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_CustomInvites_SendCustomInvite(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void SendRequestToJoin(ref SendRequestToJoinOptions options, object clientData, OnSendRequestToJoinCallback completionDelegate)
	{
		SendRequestToJoinOptionsInternal optionsInternal = default(SendRequestToJoinOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendRequestToJoinCallbackInternal completionDelegateInternal = OnSendRequestToJoinCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_CustomInvites_SendRequestToJoin(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result SetCustomInvite(ref SetCustomInviteOptions options)
	{
		SetCustomInviteOptionsInternal optionsInternal = default(SetCustomInviteOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_CustomInvites_SetCustomInvite(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	[MonoPInvokeCallback(typeof(OnAcceptRequestToJoinCallbackInternal))]
	internal static void OnAcceptRequestToJoinCallbackInternalImplementation(ref AcceptRequestToJoinCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<AcceptRequestToJoinCallbackInfoInternal, OnAcceptRequestToJoinCallback, AcceptRequestToJoinCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnCustomInviteAcceptedCallbackInternal))]
	internal static void OnCustomInviteAcceptedCallbackInternalImplementation(ref OnCustomInviteAcceptedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnCustomInviteAcceptedCallbackInfoInternal, OnCustomInviteAcceptedCallback, OnCustomInviteAcceptedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnCustomInviteReceivedCallbackInternal))]
	internal static void OnCustomInviteReceivedCallbackInternalImplementation(ref OnCustomInviteReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnCustomInviteReceivedCallbackInfoInternal, OnCustomInviteReceivedCallback, OnCustomInviteReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnCustomInviteRejectedCallbackInternal))]
	internal static void OnCustomInviteRejectedCallbackInternalImplementation(ref CustomInviteRejectedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<CustomInviteRejectedCallbackInfoInternal, OnCustomInviteRejectedCallback, CustomInviteRejectedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRejectRequestToJoinCallbackInternal))]
	internal static void OnRejectRequestToJoinCallbackInternalImplementation(ref RejectRequestToJoinCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<RejectRequestToJoinCallbackInfoInternal, OnRejectRequestToJoinCallback, RejectRequestToJoinCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRequestToJoinAcceptedCallbackInternal))]
	internal static void OnRequestToJoinAcceptedCallbackInternalImplementation(ref OnRequestToJoinAcceptedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnRequestToJoinAcceptedCallbackInfoInternal, OnRequestToJoinAcceptedCallback, OnRequestToJoinAcceptedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRequestToJoinReceivedCallbackInternal))]
	internal static void OnRequestToJoinReceivedCallbackInternalImplementation(ref RequestToJoinReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<RequestToJoinReceivedCallbackInfoInternal, OnRequestToJoinReceivedCallback, RequestToJoinReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRequestToJoinRejectedCallbackInternal))]
	internal static void OnRequestToJoinRejectedCallbackInternalImplementation(ref OnRequestToJoinRejectedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnRequestToJoinRejectedCallbackInfoInternal, OnRequestToJoinRejectedCallback, OnRequestToJoinRejectedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRequestToJoinResponseReceivedCallbackInternal))]
	internal static void OnRequestToJoinResponseReceivedCallbackInternalImplementation(ref RequestToJoinResponseReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<RequestToJoinResponseReceivedCallbackInfoInternal, OnRequestToJoinResponseReceivedCallback, RequestToJoinResponseReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSendCustomInviteCallbackInternal))]
	internal static void OnSendCustomInviteCallbackInternalImplementation(ref SendCustomInviteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<SendCustomInviteCallbackInfoInternal, OnSendCustomInviteCallback, SendCustomInviteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSendCustomNativeInviteRequestedCallbackInternal))]
	internal static void OnSendCustomNativeInviteRequestedCallbackInternalImplementation(ref SendCustomNativeInviteRequestedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<SendCustomNativeInviteRequestedCallbackInfoInternal, OnSendCustomNativeInviteRequestedCallback, SendCustomNativeInviteRequestedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSendRequestToJoinCallbackInternal))]
	internal static void OnSendRequestToJoinCallbackInternalImplementation(ref SendRequestToJoinCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<SendRequestToJoinCallbackInfoInternal, OnSendRequestToJoinCallback, SendRequestToJoinCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
