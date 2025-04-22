using System;

namespace Epic.OnlineServices.Lobby;

public sealed class LobbyInterface : Handle
{
	public const int AddnotifyjoinlobbyacceptedApiLatest = 1;

	public const int AddnotifyleavelobbyrequestedApiLatest = 1;

	public const int AddnotifylobbyinviteacceptedApiLatest = 1;

	public const int AddnotifylobbyinvitereceivedApiLatest = 1;

	public const int AddnotifylobbyinviterejectedApiLatest = 1;

	public const int AddnotifylobbymemberstatusreceivedApiLatest = 1;

	public const int AddnotifylobbymemberupdatereceivedApiLatest = 1;

	public const int AddnotifylobbyupdatereceivedApiLatest = 1;

	public const int AddnotifyrtcroomconnectionchangedApiLatest = 2;

	public const int AddnotifysendlobbynativeinviterequestedApiLatest = 1;

	public const int AttributeApiLatest = 1;

	public const int AttributedataApiLatest = 1;

	public const int CopylobbydetailshandleApiLatest = 1;

	public const int CopylobbydetailshandlebyinviteidApiLatest = 1;

	public const int CopylobbydetailshandlebyuieventidApiLatest = 1;

	public const int CreatelobbyApiLatest = 9;

	public const int CreatelobbysearchApiLatest = 1;

	public const int DestroylobbyApiLatest = 1;

	public const int GetconnectstringApiLatest = 1;

	public const int GetconnectstringBufferSize = 256;

	public const int GetinvitecountApiLatest = 1;

	public const int GetinviteidbyindexApiLatest = 1;

	public const int GetrtcroomnameApiLatest = 1;

	public const int HardmutememberApiLatest = 1;

	public const int InviteidMaxLength = 64;

	public const int IsrtcroomconnectedApiLatest = 1;

	public const int JoinlobbyApiLatest = 4;

	public const int JoinlobbybyidApiLatest = 2;

	public const int KickmemberApiLatest = 1;

	public const int LeavelobbyApiLatest = 1;

	public const int LocalrtcoptionsApiLatest = 1;

	public const int MaxLobbies = 16;

	public const int MaxLobbyMembers = 64;

	public const int MaxLobbyidoverrideLength = 60;

	public const int MaxSearchResults = 200;

	public const int MinLobbyidoverrideLength = 4;

	public const int ParseconnectstringApiLatest = 1;

	public const int ParseconnectstringBufferSize = 256;

	public const int PromotememberApiLatest = 1;

	public const int QueryinvitesApiLatest = 1;

	public const int RejectinviteApiLatest = 1;

	public static readonly Utf8String SearchBucketId = "bucket";

	public static readonly Utf8String SearchMincurrentmembers = "mincurrentmembers";

	public static readonly Utf8String SearchMinslotsavailable = "minslotsavailable";

	public const int SendinviteApiLatest = 1;

	public const int UpdatelobbyApiLatest = 1;

	public const int UpdatelobbymodificationApiLatest = 1;

	public LobbyInterface()
	{
	}

	public LobbyInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyJoinLobbyAccepted(ref AddNotifyJoinLobbyAcceptedOptions options, object clientData, OnJoinLobbyAcceptedCallback notificationFn)
	{
		AddNotifyJoinLobbyAcceptedOptionsInternal optionsInternal = default(AddNotifyJoinLobbyAcceptedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnJoinLobbyAcceptedCallbackInternal notificationFnInternal = OnJoinLobbyAcceptedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyJoinLobbyAccepted(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLeaveLobbyRequested(ref AddNotifyLeaveLobbyRequestedOptions options, object clientData, OnLeaveLobbyRequestedCallback notificationFn)
	{
		AddNotifyLeaveLobbyRequestedOptionsInternal optionsInternal = default(AddNotifyLeaveLobbyRequestedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLeaveLobbyRequestedCallbackInternal notificationFnInternal = OnLeaveLobbyRequestedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyLeaveLobbyRequested(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLobbyInviteAccepted(ref AddNotifyLobbyInviteAcceptedOptions options, object clientData, OnLobbyInviteAcceptedCallback notificationFn)
	{
		AddNotifyLobbyInviteAcceptedOptionsInternal optionsInternal = default(AddNotifyLobbyInviteAcceptedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLobbyInviteAcceptedCallbackInternal notificationFnInternal = OnLobbyInviteAcceptedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyLobbyInviteAccepted(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLobbyInviteReceived(ref AddNotifyLobbyInviteReceivedOptions options, object clientData, OnLobbyInviteReceivedCallback notificationFn)
	{
		AddNotifyLobbyInviteReceivedOptionsInternal optionsInternal = default(AddNotifyLobbyInviteReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLobbyInviteReceivedCallbackInternal notificationFnInternal = OnLobbyInviteReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyLobbyInviteReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLobbyInviteRejected(ref AddNotifyLobbyInviteRejectedOptions options, object clientData, OnLobbyInviteRejectedCallback notificationFn)
	{
		AddNotifyLobbyInviteRejectedOptionsInternal optionsInternal = default(AddNotifyLobbyInviteRejectedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLobbyInviteRejectedCallbackInternal notificationFnInternal = OnLobbyInviteRejectedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyLobbyInviteRejected(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLobbyMemberStatusReceived(ref AddNotifyLobbyMemberStatusReceivedOptions options, object clientData, OnLobbyMemberStatusReceivedCallback notificationFn)
	{
		AddNotifyLobbyMemberStatusReceivedOptionsInternal optionsInternal = default(AddNotifyLobbyMemberStatusReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLobbyMemberStatusReceivedCallbackInternal notificationFnInternal = OnLobbyMemberStatusReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyLobbyMemberStatusReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLobbyMemberUpdateReceived(ref AddNotifyLobbyMemberUpdateReceivedOptions options, object clientData, OnLobbyMemberUpdateReceivedCallback notificationFn)
	{
		AddNotifyLobbyMemberUpdateReceivedOptionsInternal optionsInternal = default(AddNotifyLobbyMemberUpdateReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLobbyMemberUpdateReceivedCallbackInternal notificationFnInternal = OnLobbyMemberUpdateReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyLobbyMemberUpdateReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyLobbyUpdateReceived(ref AddNotifyLobbyUpdateReceivedOptions options, object clientData, OnLobbyUpdateReceivedCallback notificationFn)
	{
		AddNotifyLobbyUpdateReceivedOptionsInternal optionsInternal = default(AddNotifyLobbyUpdateReceivedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLobbyUpdateReceivedCallbackInternal notificationFnInternal = OnLobbyUpdateReceivedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyLobbyUpdateReceived(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyRTCRoomConnectionChanged(ref AddNotifyRTCRoomConnectionChangedOptions options, object clientData, OnRTCRoomConnectionChangedCallback notificationFn)
	{
		AddNotifyRTCRoomConnectionChangedOptionsInternal optionsInternal = default(AddNotifyRTCRoomConnectionChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRTCRoomConnectionChangedCallbackInternal notificationFnInternal = OnRTCRoomConnectionChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifyRTCRoomConnectionChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifySendLobbyNativeInviteRequested(ref AddNotifySendLobbyNativeInviteRequestedOptions options, object clientData, OnSendLobbyNativeInviteRequestedCallback notificationFn)
	{
		AddNotifySendLobbyNativeInviteRequestedOptionsInternal optionsInternal = default(AddNotifySendLobbyNativeInviteRequestedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendLobbyNativeInviteRequestedCallbackInternal notificationFnInternal = OnSendLobbyNativeInviteRequestedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_Lobby_AddNotifySendLobbyNativeInviteRequested(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result CopyLobbyDetailsHandle(ref CopyLobbyDetailsHandleOptions options, out LobbyDetails outLobbyDetailsHandle)
	{
		CopyLobbyDetailsHandleOptionsInternal optionsInternal = default(CopyLobbyDetailsHandleOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLobbyDetailsHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Lobby_CopyLobbyDetailsHandle(base.InnerHandle, ref optionsInternal, ref outLobbyDetailsHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outLobbyDetailsHandleAddress, out outLobbyDetailsHandle);
		return result;
	}

	public Result CopyLobbyDetailsHandleByInviteId(ref CopyLobbyDetailsHandleByInviteIdOptions options, out LobbyDetails outLobbyDetailsHandle)
	{
		CopyLobbyDetailsHandleByInviteIdOptionsInternal optionsInternal = default(CopyLobbyDetailsHandleByInviteIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLobbyDetailsHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Lobby_CopyLobbyDetailsHandleByInviteId(base.InnerHandle, ref optionsInternal, ref outLobbyDetailsHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outLobbyDetailsHandleAddress, out outLobbyDetailsHandle);
		return result;
	}

	public Result CopyLobbyDetailsHandleByUiEventId(ref CopyLobbyDetailsHandleByUiEventIdOptions options, out LobbyDetails outLobbyDetailsHandle)
	{
		CopyLobbyDetailsHandleByUiEventIdOptionsInternal optionsInternal = default(CopyLobbyDetailsHandleByUiEventIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLobbyDetailsHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Lobby_CopyLobbyDetailsHandleByUiEventId(base.InnerHandle, ref optionsInternal, ref outLobbyDetailsHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outLobbyDetailsHandleAddress, out outLobbyDetailsHandle);
		return result;
	}

	public void CreateLobby(ref CreateLobbyOptions options, object clientData, OnCreateLobbyCallback completionDelegate)
	{
		CreateLobbyOptionsInternal optionsInternal = default(CreateLobbyOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnCreateLobbyCallbackInternal completionDelegateInternal = OnCreateLobbyCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_CreateLobby(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result CreateLobbySearch(ref CreateLobbySearchOptions options, out LobbySearch outLobbySearchHandle)
	{
		CreateLobbySearchOptionsInternal optionsInternal = default(CreateLobbySearchOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLobbySearchHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Lobby_CreateLobbySearch(base.InnerHandle, ref optionsInternal, ref outLobbySearchHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outLobbySearchHandleAddress, out outLobbySearchHandle);
		return result;
	}

	public void DestroyLobby(ref DestroyLobbyOptions options, object clientData, OnDestroyLobbyCallback completionDelegate)
	{
		DestroyLobbyOptionsInternal optionsInternal = default(DestroyLobbyOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDestroyLobbyCallbackInternal completionDelegateInternal = OnDestroyLobbyCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_DestroyLobby(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result GetConnectString(ref GetConnectStringOptions options, out Utf8String outBuffer)
	{
		GetConnectStringOptionsInternal optionsInternal = default(GetConnectStringOptionsInternal);
		optionsInternal.Set(ref options);
		uint inOutBufferLength = 256u;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Lobby_GetConnectString(base.InnerHandle, ref optionsInternal, outBufferAddress, ref inOutBufferLength);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public uint GetInviteCount(ref GetInviteCountOptions options)
	{
		GetInviteCountOptionsInternal optionsInternal = default(GetInviteCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_Lobby_GetInviteCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetInviteIdByIndex(ref GetInviteIdByIndexOptions options, out Utf8String outBuffer)
	{
		GetInviteIdByIndexOptionsInternal optionsInternal = default(GetInviteIdByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		int inOutBufferLength = 65;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Lobby_GetInviteIdByIndex(base.InnerHandle, ref optionsInternal, outBufferAddress, ref inOutBufferLength);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public Result GetRTCRoomName(ref GetRTCRoomNameOptions options, out Utf8String outBuffer)
	{
		GetRTCRoomNameOptionsInternal optionsInternal = default(GetRTCRoomNameOptionsInternal);
		optionsInternal.Set(ref options);
		uint inOutBufferLength = 256u;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Lobby_GetRTCRoomName(base.InnerHandle, ref optionsInternal, outBufferAddress, ref inOutBufferLength);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public void HardMuteMember(ref HardMuteMemberOptions options, object clientData, OnHardMuteMemberCallback completionDelegate)
	{
		HardMuteMemberOptionsInternal optionsInternal = default(HardMuteMemberOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnHardMuteMemberCallbackInternal completionDelegateInternal = OnHardMuteMemberCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_HardMuteMember(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result IsRTCRoomConnected(ref IsRTCRoomConnectedOptions options, out bool bOutIsConnected)
	{
		IsRTCRoomConnectedOptionsInternal optionsInternal = default(IsRTCRoomConnectedOptionsInternal);
		optionsInternal.Set(ref options);
		int bOutIsConnectedInt = 0;
		Result result = Bindings.EOS_Lobby_IsRTCRoomConnected(base.InnerHandle, ref optionsInternal, ref bOutIsConnectedInt);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(bOutIsConnectedInt, out bOutIsConnected);
		return result;
	}

	public void JoinLobby(ref JoinLobbyOptions options, object clientData, OnJoinLobbyCallback completionDelegate)
	{
		JoinLobbyOptionsInternal optionsInternal = default(JoinLobbyOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnJoinLobbyCallbackInternal completionDelegateInternal = OnJoinLobbyCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_JoinLobby(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void JoinLobbyById(ref JoinLobbyByIdOptions options, object clientData, OnJoinLobbyByIdCallback completionDelegate)
	{
		JoinLobbyByIdOptionsInternal optionsInternal = default(JoinLobbyByIdOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnJoinLobbyByIdCallbackInternal completionDelegateInternal = OnJoinLobbyByIdCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_JoinLobbyById(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void KickMember(ref KickMemberOptions options, object clientData, OnKickMemberCallback completionDelegate)
	{
		KickMemberOptionsInternal optionsInternal = default(KickMemberOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnKickMemberCallbackInternal completionDelegateInternal = OnKickMemberCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_KickMember(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void LeaveLobby(ref LeaveLobbyOptions options, object clientData, OnLeaveLobbyCallback completionDelegate)
	{
		LeaveLobbyOptionsInternal optionsInternal = default(LeaveLobbyOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLeaveLobbyCallbackInternal completionDelegateInternal = OnLeaveLobbyCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_LeaveLobby(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result ParseConnectString(ref ParseConnectStringOptions options, out Utf8String outBuffer)
	{
		ParseConnectStringOptionsInternal optionsInternal = default(ParseConnectStringOptionsInternal);
		optionsInternal.Set(ref options);
		uint inOutBufferLength = 256u;
		IntPtr outBufferAddress = Helper.AddAllocation(inOutBufferLength);
		Result result = Bindings.EOS_Lobby_ParseConnectString(base.InnerHandle, ref optionsInternal, outBufferAddress, ref inOutBufferLength);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outBufferAddress, out outBuffer);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public void PromoteMember(ref PromoteMemberOptions options, object clientData, OnPromoteMemberCallback completionDelegate)
	{
		PromoteMemberOptionsInternal optionsInternal = default(PromoteMemberOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnPromoteMemberCallbackInternal completionDelegateInternal = OnPromoteMemberCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_PromoteMember(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryInvites(ref QueryInvitesOptions options, object clientData, OnQueryInvitesCallback completionDelegate)
	{
		QueryInvitesOptionsInternal optionsInternal = default(QueryInvitesOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryInvitesCallbackInternal completionDelegateInternal = OnQueryInvitesCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_QueryInvites(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
	{
		RejectInviteOptionsInternal optionsInternal = default(RejectInviteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRejectInviteCallbackInternal completionDelegateInternal = OnRejectInviteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_RejectInvite(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyJoinLobbyAccepted(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyJoinLobbyAccepted(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLeaveLobbyRequested(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyLeaveLobbyRequested(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLobbyInviteAccepted(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyLobbyInviteAccepted(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLobbyInviteReceived(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyLobbyInviteReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLobbyInviteRejected(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyLobbyInviteRejected(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLobbyMemberStatusReceived(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyLobbyMemberStatusReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLobbyMemberUpdateReceived(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyLobbyMemberUpdateReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyLobbyUpdateReceived(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyLobbyUpdateReceived(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifyRTCRoomConnectionChanged(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifyRTCRoomConnectionChanged(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void RemoveNotifySendLobbyNativeInviteRequested(ulong inId)
	{
		Bindings.EOS_Lobby_RemoveNotifySendLobbyNativeInviteRequested(base.InnerHandle, inId);
		Helper.RemoveCallbackByNotificationId(inId);
	}

	public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
	{
		SendInviteOptionsInternal optionsInternal = default(SendInviteOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSendInviteCallbackInternal completionDelegateInternal = OnSendInviteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_SendInvite(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateLobby(ref UpdateLobbyOptions options, object clientData, OnUpdateLobbyCallback completionDelegate)
	{
		UpdateLobbyOptionsInternal optionsInternal = default(UpdateLobbyOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateLobbyCallbackInternal completionDelegateInternal = OnUpdateLobbyCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_Lobby_UpdateLobby(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result UpdateLobbyModification(ref UpdateLobbyModificationOptions options, out LobbyModification outLobbyModificationHandle)
	{
		UpdateLobbyModificationOptionsInternal optionsInternal = default(UpdateLobbyModificationOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outLobbyModificationHandleAddress = IntPtr.Zero;
		Result result = Bindings.EOS_Lobby_UpdateLobbyModification(base.InnerHandle, ref optionsInternal, ref outLobbyModificationHandleAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outLobbyModificationHandleAddress, out outLobbyModificationHandle);
		return result;
	}

	[MonoPInvokeCallback(typeof(OnCreateLobbyCallbackInternal))]
	internal static void OnCreateLobbyCallbackInternalImplementation(ref CreateLobbyCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<CreateLobbyCallbackInfoInternal, OnCreateLobbyCallback, CreateLobbyCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnDestroyLobbyCallbackInternal))]
	internal static void OnDestroyLobbyCallbackInternalImplementation(ref DestroyLobbyCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<DestroyLobbyCallbackInfoInternal, OnDestroyLobbyCallback, DestroyLobbyCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnHardMuteMemberCallbackInternal))]
	internal static void OnHardMuteMemberCallbackInternalImplementation(ref HardMuteMemberCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<HardMuteMemberCallbackInfoInternal, OnHardMuteMemberCallback, HardMuteMemberCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnJoinLobbyAcceptedCallbackInternal))]
	internal static void OnJoinLobbyAcceptedCallbackInternalImplementation(ref JoinLobbyAcceptedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<JoinLobbyAcceptedCallbackInfoInternal, OnJoinLobbyAcceptedCallback, JoinLobbyAcceptedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnJoinLobbyByIdCallbackInternal))]
	internal static void OnJoinLobbyByIdCallbackInternalImplementation(ref JoinLobbyByIdCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<JoinLobbyByIdCallbackInfoInternal, OnJoinLobbyByIdCallback, JoinLobbyByIdCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnJoinLobbyCallbackInternal))]
	internal static void OnJoinLobbyCallbackInternalImplementation(ref JoinLobbyCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<JoinLobbyCallbackInfoInternal, OnJoinLobbyCallback, JoinLobbyCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnKickMemberCallbackInternal))]
	internal static void OnKickMemberCallbackInternalImplementation(ref KickMemberCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<KickMemberCallbackInfoInternal, OnKickMemberCallback, KickMemberCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLeaveLobbyCallbackInternal))]
	internal static void OnLeaveLobbyCallbackInternalImplementation(ref LeaveLobbyCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<LeaveLobbyCallbackInfoInternal, OnLeaveLobbyCallback, LeaveLobbyCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLeaveLobbyRequestedCallbackInternal))]
	internal static void OnLeaveLobbyRequestedCallbackInternalImplementation(ref LeaveLobbyRequestedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LeaveLobbyRequestedCallbackInfoInternal, OnLeaveLobbyRequestedCallback, LeaveLobbyRequestedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLobbyInviteAcceptedCallbackInternal))]
	internal static void OnLobbyInviteAcceptedCallbackInternalImplementation(ref LobbyInviteAcceptedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LobbyInviteAcceptedCallbackInfoInternal, OnLobbyInviteAcceptedCallback, LobbyInviteAcceptedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLobbyInviteReceivedCallbackInternal))]
	internal static void OnLobbyInviteReceivedCallbackInternalImplementation(ref LobbyInviteReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LobbyInviteReceivedCallbackInfoInternal, OnLobbyInviteReceivedCallback, LobbyInviteReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLobbyInviteRejectedCallbackInternal))]
	internal static void OnLobbyInviteRejectedCallbackInternalImplementation(ref LobbyInviteRejectedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LobbyInviteRejectedCallbackInfoInternal, OnLobbyInviteRejectedCallback, LobbyInviteRejectedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLobbyMemberStatusReceivedCallbackInternal))]
	internal static void OnLobbyMemberStatusReceivedCallbackInternalImplementation(ref LobbyMemberStatusReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LobbyMemberStatusReceivedCallbackInfoInternal, OnLobbyMemberStatusReceivedCallback, LobbyMemberStatusReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLobbyMemberUpdateReceivedCallbackInternal))]
	internal static void OnLobbyMemberUpdateReceivedCallbackInternalImplementation(ref LobbyMemberUpdateReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LobbyMemberUpdateReceivedCallbackInfoInternal, OnLobbyMemberUpdateReceivedCallback, LobbyMemberUpdateReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLobbyUpdateReceivedCallbackInternal))]
	internal static void OnLobbyUpdateReceivedCallbackInternalImplementation(ref LobbyUpdateReceivedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<LobbyUpdateReceivedCallbackInfoInternal, OnLobbyUpdateReceivedCallback, LobbyUpdateReceivedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnPromoteMemberCallbackInternal))]
	internal static void OnPromoteMemberCallbackInternalImplementation(ref PromoteMemberCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<PromoteMemberCallbackInfoInternal, OnPromoteMemberCallback, PromoteMemberCallbackInfo>(ref data, out var callback, out var callbackInfo))
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

	[MonoPInvokeCallback(typeof(OnRTCRoomConnectionChangedCallbackInternal))]
	internal static void OnRTCRoomConnectionChangedCallbackInternalImplementation(ref RTCRoomConnectionChangedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<RTCRoomConnectionChangedCallbackInfoInternal, OnRTCRoomConnectionChangedCallback, RTCRoomConnectionChangedCallbackInfo>(ref data, out var callback, out var callbackInfo))
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

	[MonoPInvokeCallback(typeof(OnSendLobbyNativeInviteRequestedCallbackInternal))]
	internal static void OnSendLobbyNativeInviteRequestedCallbackInternalImplementation(ref SendLobbyNativeInviteRequestedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<SendLobbyNativeInviteRequestedCallbackInfoInternal, OnSendLobbyNativeInviteRequestedCallback, SendLobbyNativeInviteRequestedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateLobbyCallbackInternal))]
	internal static void OnUpdateLobbyCallbackInternalImplementation(ref UpdateLobbyCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateLobbyCallbackInfoInternal, OnUpdateLobbyCallback, UpdateLobbyCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
