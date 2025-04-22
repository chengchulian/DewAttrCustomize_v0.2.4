using System;
using Epic.OnlineServices.RTCAudio;

namespace Epic.OnlineServices.RTC;

public sealed class RTCInterface : Handle
{
	public const int AddnotifydisconnectedApiLatest = 1;

	public const int AddnotifyparticipantstatuschangedApiLatest = 1;

	public const int AddnotifyroomstatisticsupdatedApiLatest = 1;

	public const int BlockparticipantApiLatest = 1;

	public const int JoinroomApiLatest = 1;

	public const int LeaveroomApiLatest = 1;

	public const int OptionApiLatest = 1;

	public const int OptionKeyMaxcharcount = 256;

	public const int OptionValueMaxcharcount = 256;

	public const int ParticipantmetadataApiLatest = 1;

	public const int ParticipantmetadataKeyMaxcharcount = 256;

	public const int ParticipantmetadataValueMaxcharcount = 256;

	public const int SetroomsettingApiLatest = 1;

	public const int SetsettingApiLatest = 1;

	public RTCInterface()
	{
	}

	public RTCInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyDisconnected(ref AddNotifyDisconnectedOptions options, object clientData, OnDisconnectedCallback completionDelegate)
	{
		AddNotifyDisconnectedOptionsInternal optionsInternal = default(AddNotifyDisconnectedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDisconnectedCallbackInternal completionDelegateInternal = OnDisconnectedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		ulong funcResult = Bindings.EOS_RTC_AddNotifyDisconnected(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyParticipantStatusChanged(ref AddNotifyParticipantStatusChangedOptions options, object clientData, OnParticipantStatusChangedCallback completionDelegate)
	{
		AddNotifyParticipantStatusChangedOptionsInternal optionsInternal = default(AddNotifyParticipantStatusChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnParticipantStatusChangedCallbackInternal completionDelegateInternal = OnParticipantStatusChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		ulong funcResult = Bindings.EOS_RTC_AddNotifyParticipantStatusChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyRoomStatisticsUpdated(ref AddNotifyRoomStatisticsUpdatedOptions options, object clientData, OnRoomStatisticsUpdatedCallback statisticsUpdateHandler)
	{
		AddNotifyRoomStatisticsUpdatedOptionsInternal optionsInternal = default(AddNotifyRoomStatisticsUpdatedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRoomStatisticsUpdatedCallbackInternal statisticsUpdateHandlerInternal = OnRoomStatisticsUpdatedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, statisticsUpdateHandler, statisticsUpdateHandlerInternal);
		ulong funcResult = Bindings.EOS_RTC_AddNotifyRoomStatisticsUpdated(base.InnerHandle, ref optionsInternal, clientDataAddress, statisticsUpdateHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public void BlockParticipant(ref BlockParticipantOptions options, object clientData, OnBlockParticipantCallback completionDelegate)
	{
		BlockParticipantOptionsInternal optionsInternal = default(BlockParticipantOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnBlockParticipantCallbackInternal completionDelegateInternal = OnBlockParticipantCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTC_BlockParticipant(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public RTCAudioInterface GetAudioInterface()
	{
		Helper.Get(Bindings.EOS_RTC_GetAudioInterface(base.InnerHandle), out RTCAudioInterface funcResultReturn);
		return funcResultReturn;
	}

	public void JoinRoom(ref JoinRoomOptions options, object clientData, OnJoinRoomCallback completionDelegate)
	{
		JoinRoomOptionsInternal optionsInternal = default(JoinRoomOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnJoinRoomCallbackInternal completionDelegateInternal = OnJoinRoomCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTC_JoinRoom(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void LeaveRoom(ref LeaveRoomOptions options, object clientData, OnLeaveRoomCallback completionDelegate)
	{
		LeaveRoomOptionsInternal optionsInternal = default(LeaveRoomOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnLeaveRoomCallbackInternal completionDelegateInternal = OnLeaveRoomCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTC_LeaveRoom(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyDisconnected(ulong notificationId)
	{
		Bindings.EOS_RTC_RemoveNotifyDisconnected(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyParticipantStatusChanged(ulong notificationId)
	{
		Bindings.EOS_RTC_RemoveNotifyParticipantStatusChanged(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyRoomStatisticsUpdated(ulong notificationId)
	{
		Bindings.EOS_RTC_RemoveNotifyRoomStatisticsUpdated(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public Result SetRoomSetting(ref SetRoomSettingOptions options)
	{
		SetRoomSettingOptionsInternal optionsInternal = default(SetRoomSettingOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_RTC_SetRoomSetting(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetSetting(ref SetSettingOptions options)
	{
		SetSettingOptionsInternal optionsInternal = default(SetSettingOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_RTC_SetSetting(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	[MonoPInvokeCallback(typeof(OnBlockParticipantCallbackInternal))]
	internal static void OnBlockParticipantCallbackInternalImplementation(ref BlockParticipantCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<BlockParticipantCallbackInfoInternal, OnBlockParticipantCallback, BlockParticipantCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnDisconnectedCallbackInternal))]
	internal static void OnDisconnectedCallbackInternalImplementation(ref DisconnectedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<DisconnectedCallbackInfoInternal, OnDisconnectedCallback, DisconnectedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnJoinRoomCallbackInternal))]
	internal static void OnJoinRoomCallbackInternalImplementation(ref JoinRoomCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<JoinRoomCallbackInfoInternal, OnJoinRoomCallback, JoinRoomCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnLeaveRoomCallbackInternal))]
	internal static void OnLeaveRoomCallbackInternalImplementation(ref LeaveRoomCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<LeaveRoomCallbackInfoInternal, OnLeaveRoomCallback, LeaveRoomCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnParticipantStatusChangedCallbackInternal))]
	internal static void OnParticipantStatusChangedCallbackInternalImplementation(ref ParticipantStatusChangedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<ParticipantStatusChangedCallbackInfoInternal, OnParticipantStatusChangedCallback, ParticipantStatusChangedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRoomStatisticsUpdatedCallbackInternal))]
	internal static void OnRoomStatisticsUpdatedCallbackInternalImplementation(ref RoomStatisticsUpdatedInfoInternal data)
	{
		if (Helper.TryGetCallback<RoomStatisticsUpdatedInfoInternal, OnRoomStatisticsUpdatedCallback, RoomStatisticsUpdatedInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
