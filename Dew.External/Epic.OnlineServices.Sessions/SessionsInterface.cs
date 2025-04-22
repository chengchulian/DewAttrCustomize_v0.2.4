using System;

namespace Epic.OnlineServices.Sessions;

public sealed class SessionsInterface : Handle
{
	public const int AddnotifyjoinsessionacceptedApiLatest = 1;

	public const int AddnotifyleavesessionrequestedApiLatest = 1;

	public const int AddnotifysendsessionnativeinviterequestedApiLatest = 1;

	public const int AddnotifysessioninviteacceptedApiLatest = 1;

	public const int AddnotifysessioninvitereceivedApiLatest = 1;

	public const int AddnotifysessioninviterejectedApiLatest = 1;

	public const int AttributedataApiLatest = 1;

	public const int CopyactivesessionhandleApiLatest = 1;

	public const int CopysessionhandlebyinviteidApiLatest = 1;

	public const int CopysessionhandlebyuieventidApiLatest = 1;

	public const int CopysessionhandleforpresenceApiLatest = 1;

	public const int CreatesessionmodificationApiLatest = 5;

	public const int CreatesessionsearchApiLatest = 1;

	public const int DestroysessionApiLatest = 1;

	public const int DumpsessionstateApiLatest = 1;

	public const int EndsessionApiLatest = 1;

	public const int GetinvitecountApiLatest = 1;

	public const int GetinviteidbyindexApiLatest = 1;

	public const int InviteidMaxLength = 64;

	public const int IsuserinsessionApiLatest = 1;

	public const int JoinsessionApiLatest = 2;

	public const int MaxSearchResults = 200;

	public const int Maxregisteredplayers = 1000;

	public const int QueryinvitesApiLatest = 1;

	public const int RegisterplayersApiLatest = 3;

	public const int RejectinviteApiLatest = 1;

	public static readonly Utf8String SearchBucketId = "bucket";

	public static readonly Utf8String SearchEmptyServersOnly = "emptyonly";

	public static readonly Utf8String SearchMinslotsavailable = "minslotsavailable";

	public static readonly Utf8String SearchNonemptyServersOnly = "nonemptyonly";

	public const int SendinviteApiLatest = 1;

	public const int SessionattributeApiLatest = 1;

	public const int SessionattributedataApiLatest = 1;

	public const int StartsessionApiLatest = 1;

	public const int UnregisterplayersApiLatest = 2;

	public const int UpdatesessionApiLatest = 1;

	public const int UpdatesessionmodificationApiLatest = 1;

	public SessionsInterface()
	{
	}

	public SessionsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyJoinSessionAccepted(ref AddNotifyJoinSessionAcceptedOptions options, object clientData, OnJoinSessionAcceptedCallback notificationFn)
	{
		AddNotifyJoinSessionAcceptedOptionsInternal optionsInternal = default(AddNotifyJoinSessionAcceptedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnJoinSessionAcceptedCallbackInternal notificationFnInternal = OnJoinSessionAcceptedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Sessions_AddNotifyJoinSessionAccepted(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLeaveSessionRequested(ref AddNotifyLeaveSessionRequestedOptions options, object clientData, OnLeaveSessionRequestedCallback notificationFn)
	{
		AddNotifyLeaveSessionRequestedOptionsInternal optionsInternal = default(AddNotifyLeaveSessionRequestedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLeaveSessionRequestedCallbackInternal notificationFnInternal = OnLeaveSessionRequestedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Sessions_AddNotifyLeaveSessionRequested(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifySendSessionNativeInviteRequested(ref AddNotifySendSessionNativeInviteRequestedOptions options, object clientData, OnSendSessionNativeInviteRequestedCallback notificationFn)
	{
		AddNotifySendSessionNativeInviteRequestedOptionsInternal optionsInternal = default(AddNotifySendSessionNativeInviteRequestedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendSessionNativeInviteRequestedCallbackInternal notificationFnInternal = OnSendSessionNativeInviteRequestedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Sessions_AddNotifySendSessionNativeInviteRequested(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifySessionInviteAccepted(ref AddNotifySessionInviteAcceptedOptions options, object clientData, OnSessionInviteAcceptedCallback notificationFn)
	{
		AddNotifySessionInviteAcceptedOptionsInternal optionsInternal = default(AddNotifySessionInviteAcceptedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSessionInviteAcceptedCallbackInternal notificationFnInternal = OnSessionInviteAcceptedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Sessions_AddNotifySessionInviteAccepted(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifySessionInviteReceived(ref AddNotifySessionInviteReceivedOptions options, object clientData, OnSessionInviteReceivedCallback notificationFn)
	{
		AddNotifySessionInviteReceivedOptionsInternal optionsInternal = default(AddNotifySessionInviteReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSessionInviteReceivedCallbackInternal notificationFnInternal = OnSessionInviteReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Sessions_AddNotifySessionInviteReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifySessionInviteRejected(ref AddNotifySessionInviteRejectedOptions options, object clientData, OnSessionInviteRejectedCallback notificationFn)
	{
		AddNotifySessionInviteRejectedOptionsInternal optionsInternal = default(AddNotifySessionInviteRejectedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSessionInviteRejectedCallbackInternal notificationFnInternal = OnSessionInviteRejectedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Sessions_AddNotifySessionInviteRejected(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result CopyActiveSessionHandle(ref CopyActiveSessionHandleOptions options, out ActiveSession outSessionHandle)
	{
		CopyActiveSessionHandleOptionsInternal optionsInternal = default(CopyActiveSessionHandleOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Sessions_CopyActiveSessionHandle(base.InnerHandle, ref optionsInternal, ref outSessionHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outSessionHandleAddress, out outSessionHandle);
		return result;
	}

	public Result CopySessionHandleByInviteId(ref CopySessionHandleByInviteIdOptions options, out SessionDetails outSessionHandle)
	{
		CopySessionHandleByInviteIdOptionsInternal optionsInternal = default(CopySessionHandleByInviteIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Sessions_CopySessionHandleByInviteId(base.InnerHandle, ref optionsInternal, ref outSessionHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outSessionHandleAddress, out outSessionHandle);
		return result;
	}

	public Result CopySessionHandleByUiEventId(ref CopySessionHandleByUiEventIdOptions options, out SessionDetails outSessionHandle)
	{
		CopySessionHandleByUiEventIdOptionsInternal optionsInternal = default(CopySessionHandleByUiEventIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Sessions_CopySessionHandleByUiEventId(base.InnerHandle, ref optionsInternal, ref outSessionHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outSessionHandleAddress, out outSessionHandle);
		return result;
	}

	public Result CopySessionHandleForPresence(ref CopySessionHandleForPresenceOptions options, out SessionDetails outSessionHandle)
	{
		CopySessionHandleForPresenceOptionsInternal optionsInternal = default(CopySessionHandleForPresenceOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Sessions_CopySessionHandleForPresence(base.InnerHandle, ref optionsInternal, ref outSessionHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outSessionHandleAddress, out outSessionHandle);
		return result;
	}

	public Result CreateSessionModification(ref CreateSessionModificationOptions options, out SessionModification outSessionModificationHandle)
	{
		CreateSessionModificationOptionsInternal optionsInternal = default(CreateSessionModificationOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionModificationHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Sessions_CreateSessionModification(base.InnerHandle, ref optionsInternal, ref outSessionModificationHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outSessionModificationHandleAddress, out outSessionModificationHandle);
		return result;
	}

	public Result CreateSessionSearch(ref CreateSessionSearchOptions options, out SessionSearch outSessionSearchHandle)
	{
		CreateSessionSearchOptionsInternal optionsInternal = default(CreateSessionSearchOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionSearchHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Sessions_CreateSessionSearch(base.InnerHandle, ref optionsInternal, ref outSessionSearchHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outSessionSearchHandleAddress, out outSessionSearchHandle);
		return result;
	}

	public void DestroySession(ref DestroySessionOptions options, object clientData, OnDestroySessionCallback completionDelegate)
	{
		DestroySessionOptionsInternal optionsInternal = default(DestroySessionOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDestroySessionCallbackInternal completionDelegateInternal = OnDestroySessionCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_DestroySession(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result DumpSessionState(ref DumpSessionStateOptions options)
	{
		DumpSessionStateOptionsInternal optionsInternal = default(DumpSessionStateOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_Sessions_DumpSessionState(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void EndSession(ref EndSessionOptions options, object clientData, OnEndSessionCallback completionDelegate)
	{
		EndSessionOptionsInternal optionsInternal = default(EndSessionOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnEndSessionCallbackInternal completionDelegateInternal = OnEndSessionCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_EndSession(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public uint GetInviteCount(ref GetInviteCountOptions options)
	{
		GetInviteCountOptionsInternal optionsInternal = default(GetInviteCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Sessions_GetInviteCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetInviteIdByIndex(ref GetInviteIdByIndexOptions options, out Utf8String outBuffer)
	{
		GetInviteIdByIndexOptionsInternal optionsInternal = default(GetInviteIdByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		int inOutBufferLength = 65;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Sessions_GetInviteIdByIndex(base.InnerHandle, ref optionsInternal, outBufferAddress, ref inOutBufferLength);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public Result IsUserInSession(ref IsUserInSessionOptions options)
	{
		IsUserInSessionOptionsInternal optionsInternal = default(IsUserInSessionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_Sessions_IsUserInSession(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void JoinSession(ref JoinSessionOptions options, object clientData, OnJoinSessionCallback completionDelegate)
	{
		JoinSessionOptionsInternal optionsInternal = default(JoinSessionOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnJoinSessionCallbackInternal completionDelegateInternal = OnJoinSessionCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_JoinSession(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryInvites(ref QueryInvitesOptions options, object clientData, OnQueryInvitesCallback completionDelegate)
	{
		QueryInvitesOptionsInternal optionsInternal = default(QueryInvitesOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryInvitesCallbackInternal completionDelegateInternal = OnQueryInvitesCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_QueryInvites(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RegisterPlayers(ref RegisterPlayersOptions options, object clientData, OnRegisterPlayersCallback completionDelegate)
	{
		RegisterPlayersOptionsInternal optionsInternal = default(RegisterPlayersOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRegisterPlayersCallbackInternal completionDelegateInternal = OnRegisterPlayersCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_RegisterPlayers(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
	{
		RejectInviteOptionsInternal optionsInternal = default(RejectInviteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRejectInviteCallbackInternal completionDelegateInternal = OnRejectInviteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_RejectInvite(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyJoinSessionAccepted(ulong inId)
	{
		Bindings.EOS_Sessions_RemoveNotifyJoinSessionAccepted(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLeaveSessionRequested(ulong inId)
	{
		Bindings.EOS_Sessions_RemoveNotifyLeaveSessionRequested(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifySendSessionNativeInviteRequested(ulong inId)
	{
		Bindings.EOS_Sessions_RemoveNotifySendSessionNativeInviteRequested(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifySessionInviteAccepted(ulong inId)
	{
		Bindings.EOS_Sessions_RemoveNotifySessionInviteAccepted(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifySessionInviteReceived(ulong inId)
	{
		Bindings.EOS_Sessions_RemoveNotifySessionInviteReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifySessionInviteRejected(ulong inId)
	{
		Bindings.EOS_Sessions_RemoveNotifySessionInviteRejected(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
	{
		SendInviteOptionsInternal optionsInternal = default(SendInviteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendInviteCallbackInternal completionDelegateInternal = OnSendInviteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_SendInvite(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void StartSession(ref StartSessionOptions options, object clientData, OnStartSessionCallback completionDelegate)
	{
		StartSessionOptionsInternal optionsInternal = default(StartSessionOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnStartSessionCallbackInternal completionDelegateInternal = OnStartSessionCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_StartSession(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UnregisterPlayers(ref UnregisterPlayersOptions options, object clientData, OnUnregisterPlayersCallback completionDelegate)
	{
		UnregisterPlayersOptionsInternal optionsInternal = default(UnregisterPlayersOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUnregisterPlayersCallbackInternal completionDelegateInternal = OnUnregisterPlayersCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_UnregisterPlayers(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateSession(ref UpdateSessionOptions options, object clientData, OnUpdateSessionCallback completionDelegate)
	{
		UpdateSessionOptionsInternal optionsInternal = default(UpdateSessionOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateSessionCallbackInternal completionDelegateInternal = OnUpdateSessionCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Sessions_UpdateSession(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result UpdateSessionModification(ref UpdateSessionModificationOptions options, out SessionModification outSessionModificationHandle)
	{
		UpdateSessionModificationOptionsInternal optionsInternal = default(UpdateSessionModificationOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outSessionModificationHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Sessions_UpdateSessionModification(base.InnerHandle, ref optionsInternal, ref outSessionModificationHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outSessionModificationHandleAddress, out outSessionModificationHandle);
		return result;
	}

	[MonoPInvokeCallback(typeof(OnDestroySessionCallbackInternal))]
	internal static void OnDestroySessionCallbackInternalImplementation(ref DestroySessionCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<DestroySessionCallbackInfoInternal, OnDestroySessionCallback, DestroySessionCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnEndSessionCallbackInternal))]
	internal static void OnEndSessionCallbackInternalImplementation(ref EndSessionCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<EndSessionCallbackInfoInternal, OnEndSessionCallback, EndSessionCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnJoinSessionAcceptedCallbackInternal))]
	internal static void OnJoinSessionAcceptedCallbackInternalImplementation(ref JoinSessionAcceptedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<JoinSessionAcceptedCallbackInfoInternal, OnJoinSessionAcceptedCallback, JoinSessionAcceptedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnJoinSessionCallbackInternal))]
	internal static void OnJoinSessionCallbackInternalImplementation(ref JoinSessionCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<JoinSessionCallbackInfoInternal, OnJoinSessionCallback, JoinSessionCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLeaveSessionRequestedCallbackInternal))]
	internal static void OnLeaveSessionRequestedCallbackInternalImplementation(ref LeaveSessionRequestedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LeaveSessionRequestedCallbackInfoInternal, OnLeaveSessionRequestedCallback, LeaveSessionRequestedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryInvitesCallbackInternal))]
	internal static void OnQueryInvitesCallbackInternalImplementation(ref QueryInvitesCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryInvitesCallbackInfoInternal, OnQueryInvitesCallback, QueryInvitesCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRegisterPlayersCallbackInternal))]
	internal static void OnRegisterPlayersCallbackInternalImplementation(ref RegisterPlayersCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<RegisterPlayersCallbackInfoInternal, OnRegisterPlayersCallback, RegisterPlayersCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRejectInviteCallbackInternal))]
	internal static void OnRejectInviteCallbackInternalImplementation(ref RejectInviteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<RejectInviteCallbackInfoInternal, OnRejectInviteCallback, RejectInviteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSendInviteCallbackInternal))]
	internal static void OnSendInviteCallbackInternalImplementation(ref SendInviteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<SendInviteCallbackInfoInternal, OnSendInviteCallback, SendInviteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSendSessionNativeInviteRequestedCallbackInternal))]
	internal static void OnSendSessionNativeInviteRequestedCallbackInternalImplementation(ref SendSessionNativeInviteRequestedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<SendSessionNativeInviteRequestedCallbackInfoInternal, OnSendSessionNativeInviteRequestedCallback, SendSessionNativeInviteRequestedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSessionInviteAcceptedCallbackInternal))]
	internal static void OnSessionInviteAcceptedCallbackInternalImplementation(ref SessionInviteAcceptedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<SessionInviteAcceptedCallbackInfoInternal, OnSessionInviteAcceptedCallback, SessionInviteAcceptedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSessionInviteReceivedCallbackInternal))]
	internal static void OnSessionInviteReceivedCallbackInternalImplementation(ref SessionInviteReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<SessionInviteReceivedCallbackInfoInternal, OnSessionInviteReceivedCallback, SessionInviteReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSessionInviteRejectedCallbackInternal))]
	internal static void OnSessionInviteRejectedCallbackInternalImplementation(ref SessionInviteRejectedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<SessionInviteRejectedCallbackInfoInternal, OnSessionInviteRejectedCallback, SessionInviteRejectedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnStartSessionCallbackInternal))]
	internal static void OnStartSessionCallbackInternalImplementation(ref StartSessionCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<StartSessionCallbackInfoInternal, OnStartSessionCallback, StartSessionCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUnregisterPlayersCallbackInternal))]
	internal static void OnUnregisterPlayersCallbackInternalImplementation(ref UnregisterPlayersCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UnregisterPlayersCallbackInfoInternal, OnUnregisterPlayersCallback, UnregisterPlayersCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateSessionCallbackInternal))]
	internal static void OnUpdateSessionCallbackInternalImplementation(ref UpdateSessionCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateSessionCallbackInfoInternal, OnUpdateSessionCallback, UpdateSessionCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
