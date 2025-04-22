using System;

namespace Epic.OnlineServices.Friends;

public sealed class FriendsInterface : Handle
{
	public const int AcceptinviteApiLatest = 1;

	public const int AddnotifyblockedusersupdateApiLatest = 1;

	public const int AddnotifyfriendsupdateApiLatest = 1;

	public const int GetblockeduseratindexApiLatest = 1;

	public const int GetblockeduserscountApiLatest = 1;

	public const int GetfriendatindexApiLatest = 1;

	public const int GetfriendscountApiLatest = 1;

	public const int GetstatusApiLatest = 1;

	public const int QueryfriendsApiLatest = 1;

	public const int RejectinviteApiLatest = 1;

	public const int SendinviteApiLatest = 1;

	public FriendsInterface()
	{
	}

	public FriendsInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public void AcceptInvite(ref AcceptInviteOptions options, object clientData, OnAcceptInviteCallback completionDelegate)
	{
		AcceptInviteOptionsInternal optionsInternal = default(AcceptInviteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAcceptInviteCallbackInternal completionDelegateInternal = OnAcceptInviteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Friends_AcceptInvite(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public ulong AddNotifyBlockedUsersUpdate(ref AddNotifyBlockedUsersUpdateOptions options, object clientData, OnBlockedUsersUpdateCallback blockedUsersUpdateHandler)
	{
		AddNotifyBlockedUsersUpdateOptionsInternal optionsInternal = default(AddNotifyBlockedUsersUpdateOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnBlockedUsersUpdateCallbackInternal blockedUsersUpdateHandlerInternal = OnBlockedUsersUpdateCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, blockedUsersUpdateHandler, blockedUsersUpdateHandlerInternal);
		ulong funcResult = Bindings.EOS_Friends_AddNotifyBlockedUsersUpdate(base.InnerHandle, ref optionsInternal, clientDataAddress, blockedUsersUpdateHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyFriendsUpdate(ref AddNotifyFriendsUpdateOptions options, object clientData, OnFriendsUpdateCallback friendsUpdateHandler)
	{
		AddNotifyFriendsUpdateOptionsInternal optionsInternal = default(AddNotifyFriendsUpdateOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnFriendsUpdateCallbackInternal friendsUpdateHandlerInternal = OnFriendsUpdateCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, friendsUpdateHandler, friendsUpdateHandlerInternal);
		ulong funcResult = Bindings.EOS_Friends_AddNotifyFriendsUpdate(base.InnerHandle, ref optionsInternal, clientDataAddress, friendsUpdateHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public EpicAccountId GetBlockedUserAtIndex(ref GetBlockedUserAtIndexOptions options)
	{
		GetBlockedUserAtIndexOptionsInternal optionsInternal = default(GetBlockedUserAtIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_Friends_GetBlockedUserAtIndex(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out EpicAccountId funcResultReturn);
		return funcResultReturn;
	}

	public int GetBlockedUsersCount(ref GetBlockedUsersCountOptions options)
	{
		GetBlockedUsersCountOptionsInternal optionsInternal = default(GetBlockedUsersCountOptionsInternal);
		optionsInternal.Set(ref options);
		int result = Bindings.EOS_Friends_GetBlockedUsersCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public EpicAccountId GetFriendAtIndex(ref GetFriendAtIndexOptions options)
	{
		GetFriendAtIndexOptionsInternal optionsInternal = default(GetFriendAtIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_Friends_GetFriendAtIndex(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out EpicAccountId funcResultReturn);
		return funcResultReturn;
	}

	public int GetFriendsCount(ref GetFriendsCountOptions options)
	{
		GetFriendsCountOptionsInternal optionsInternal = default(GetFriendsCountOptionsInternal);
		optionsInternal.Set(ref options);
		int result = Bindings.EOS_Friends_GetFriendsCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public FriendsStatus GetStatus(ref GetStatusOptions options)
	{
		GetStatusOptionsInternal optionsInternal = default(GetStatusOptionsInternal);
		optionsInternal.Set(ref options);
		FriendsStatus result = Bindings.EOS_Friends_GetStatus(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryFriends(ref QueryFriendsOptions options, object clientData, OnQueryFriendsCallback completionDelegate)
	{
		QueryFriendsOptionsInternal optionsInternal = default(QueryFriendsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryFriendsCallbackInternal completionDelegateInternal = OnQueryFriendsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Friends_QueryFriends(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
	{
		RejectInviteOptionsInternal optionsInternal = default(RejectInviteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRejectInviteCallbackInternal completionDelegateInternal = OnRejectInviteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Friends_RejectInvite(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyBlockedUsersUpdate(ulong notificationId)
	{
		Bindings.EOS_Friends_RemoveNotifyBlockedUsersUpdate(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyFriendsUpdate(ulong notificationId)
	{
		Bindings.EOS_Friends_RemoveNotifyFriendsUpdate(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
	{
		SendInviteOptionsInternal optionsInternal = default(SendInviteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendInviteCallbackInternal completionDelegateInternal = OnSendInviteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Friends_SendInvite(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnAcceptInviteCallbackInternal))]
	internal static void OnAcceptInviteCallbackInternalImplementation(ref AcceptInviteCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<AcceptInviteCallbackInfoInternal, OnAcceptInviteCallback, AcceptInviteCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnBlockedUsersUpdateCallbackInternal))]
	internal static void OnBlockedUsersUpdateCallbackInternalImplementation(ref OnBlockedUsersUpdateInfoInternal data)
	{
		if (Helper.TryGetCallback<OnBlockedUsersUpdateInfoInternal, OnBlockedUsersUpdateCallback, OnBlockedUsersUpdateInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnFriendsUpdateCallbackInternal))]
	internal static void OnFriendsUpdateCallbackInternalImplementation(ref OnFriendsUpdateInfoInternal data)
	{
		if (Helper.TryGetCallback<OnFriendsUpdateInfoInternal, OnFriendsUpdateCallback, OnFriendsUpdateInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryFriendsCallbackInternal))]
	internal static void OnQueryFriendsCallbackInternalImplementation(ref QueryFriendsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<QueryFriendsCallbackInfoInternal, OnQueryFriendsCallback, QueryFriendsCallbackInfo>(ref data, out var callback, out var callbackInfo))
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
}
