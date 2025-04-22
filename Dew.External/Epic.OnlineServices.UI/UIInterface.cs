using System;

namespace Epic.OnlineServices.UI;

public sealed class UIInterface : Handle
{
	public const int AcknowledgecorrelationidApiLatest = 1;

	public const int AcknowledgeeventidApiLatest = 1;

	public const int AddnotifydisplaysettingsupdatedApiLatest = 1;

	public const int AddnotifymemorymonitorApiLatest = 1;

	public const int AddnotifymemorymonitoroptionsApiLatest = 1;

	public const int EventidInvalid = 0;

	public const int GetfriendsexclusiveinputApiLatest = 1;

	public const int GetfriendsvisibleApiLatest = 1;

	public const int GettogglefriendsbuttonApiLatest = 1;

	public const int GettogglefriendskeyApiLatest = 1;

	public const int HidefriendsApiLatest = 1;

	public const int IssocialoverlaypausedApiLatest = 1;

	public const int MemorymonitorcallbackinfoApiLatest = 1;

	public const int PausesocialoverlayApiLatest = 1;

	public const int PrepresentApiLatest = 1;

	public const int RectApiLatest = 1;

	public const int ReportinputstateApiLatest = 2;

	public const int SetdisplaypreferenceApiLatest = 1;

	public const int SettogglefriendsbuttonApiLatest = 1;

	public const int SettogglefriendskeyApiLatest = 1;

	public const int ShowblockplayerApiLatest = 1;

	public const int ShowfriendsApiLatest = 1;

	public const int ShownativeprofileApiLatest = 1;

	public const int ShowreportplayerApiLatest = 1;

	public UIInterface()
	{
	}

	public UIInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result AcknowledgeEventId(ref AcknowledgeEventIdOptions options)
	{
		AcknowledgeEventIdOptionsInternal optionsInternal = default(AcknowledgeEventIdOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_UI_AcknowledgeEventId(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public ulong AddNotifyDisplaySettingsUpdated(ref AddNotifyDisplaySettingsUpdatedOptions options, object clientData, OnDisplaySettingsUpdatedCallback notificationFn)
	{
		AddNotifyDisplaySettingsUpdatedOptionsInternal optionsInternal = default(AddNotifyDisplaySettingsUpdatedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnDisplaySettingsUpdatedCallbackInternal notificationFnInternal = OnDisplaySettingsUpdatedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_UI_AddNotifyDisplaySettingsUpdated(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyMemoryMonitor(ref AddNotifyMemoryMonitorOptions options, object clientData, OnMemoryMonitorCallback notificationFn)
	{
		AddNotifyMemoryMonitorOptionsInternal optionsInternal = default(AddNotifyMemoryMonitorOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnMemoryMonitorCallbackInternal notificationFnInternal = OnMemoryMonitorCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_UI_AddNotifyMemoryMonitor(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public bool GetFriendsExclusiveInput(ref GetFriendsExclusiveInputOptions options)
	{
		GetFriendsExclusiveInputOptionsInternal optionsInternal = default(GetFriendsExclusiveInputOptionsInternal);
		optionsInternal.Set(ref options);
		int from = Bindings.EOS_UI_GetFriendsExclusiveInput(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out var funcResultReturn);
		return funcResultReturn;
	}

	public bool GetFriendsVisible(ref GetFriendsVisibleOptions options)
	{
		GetFriendsVisibleOptionsInternal optionsInternal = default(GetFriendsVisibleOptionsInternal);
		optionsInternal.Set(ref options);
		int from = Bindings.EOS_UI_GetFriendsVisible(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out var funcResultReturn);
		return funcResultReturn;
	}

	public NotificationLocation GetNotificationLocationPreference()
	{
		return Bindings.EOS_UI_GetNotificationLocationPreference(base.InnerHandle);
	}

	public InputStateButtonFlags GetToggleFriendsButton(ref GetToggleFriendsButtonOptions options)
	{
		GetToggleFriendsButtonOptionsInternal optionsInternal = default(GetToggleFriendsButtonOptionsInternal);
		optionsInternal.Set(ref options);
		InputStateButtonFlags result = Bindings.EOS_UI_GetToggleFriendsButton(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public KeyCombination GetToggleFriendsKey(ref GetToggleFriendsKeyOptions options)
	{
		GetToggleFriendsKeyOptionsInternal optionsInternal = default(GetToggleFriendsKeyOptionsInternal);
		optionsInternal.Set(ref options);
		KeyCombination result = Bindings.EOS_UI_GetToggleFriendsKey(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void HideFriends(ref HideFriendsOptions options, object clientData, OnHideFriendsCallback completionDelegate)
	{
		HideFriendsOptionsInternal optionsInternal = default(HideFriendsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnHideFriendsCallbackInternal completionDelegateInternal = OnHideFriendsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_UI_HideFriends(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public bool IsSocialOverlayPaused(ref IsSocialOverlayPausedOptions options)
	{
		IsSocialOverlayPausedOptionsInternal optionsInternal = default(IsSocialOverlayPausedOptionsInternal);
		optionsInternal.Set(ref options);
		int from = Bindings.EOS_UI_IsSocialOverlayPaused(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(from, out var funcResultReturn);
		return funcResultReturn;
	}

	public bool IsValidButtonCombination(InputStateButtonFlags buttonCombination)
	{
		Helper.Get(Bindings.EOS_UI_IsValidButtonCombination(base.InnerHandle, buttonCombination), out var funcResultReturn);
		return funcResultReturn;
	}

	public bool IsValidKeyCombination(KeyCombination keyCombination)
	{
		Helper.Get(Bindings.EOS_UI_IsValidKeyCombination(base.InnerHandle, keyCombination), out var funcResultReturn);
		return funcResultReturn;
	}

	public Result PauseSocialOverlay(ref PauseSocialOverlayOptions options)
	{
		PauseSocialOverlayOptionsInternal optionsInternal = default(PauseSocialOverlayOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_UI_PauseSocialOverlay(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result PrePresent(ref PrePresentOptions options)
	{
		PrePresentOptionsInternal optionsInternal = default(PrePresentOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_UI_PrePresent(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void RemoveNotifyDisplaySettingsUpdated(ulong id)
	{
		Bindings.EOS_UI_RemoveNotifyDisplaySettingsUpdated(base.InnerHandle, id);
		Helper.RemoveCallbackByNotificationId(id);
	}

	public void RemoveNotifyMemoryMonitor(ulong id)
	{
		Bindings.EOS_UI_RemoveNotifyMemoryMonitor(base.InnerHandle, id);
		Helper.RemoveCallbackByNotificationId(id);
	}

	public Result ReportInputState(ref ReportInputStateOptions options)
	{
		ReportInputStateOptionsInternal optionsInternal = default(ReportInputStateOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_UI_ReportInputState(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetDisplayPreference(ref SetDisplayPreferenceOptions options)
	{
		SetDisplayPreferenceOptionsInternal optionsInternal = default(SetDisplayPreferenceOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_UI_SetDisplayPreference(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetToggleFriendsButton(ref SetToggleFriendsButtonOptions options)
	{
		SetToggleFriendsButtonOptionsInternal optionsInternal = default(SetToggleFriendsButtonOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_UI_SetToggleFriendsButton(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetToggleFriendsKey(ref SetToggleFriendsKeyOptions options)
	{
		SetToggleFriendsKeyOptionsInternal optionsInternal = default(SetToggleFriendsKeyOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_UI_SetToggleFriendsKey(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void ShowBlockPlayer(ref ShowBlockPlayerOptions options, object clientData, OnShowBlockPlayerCallback completionDelegate)
	{
		ShowBlockPlayerOptionsInternal optionsInternal = default(ShowBlockPlayerOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnShowBlockPlayerCallbackInternal completionDelegateInternal = OnShowBlockPlayerCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_UI_ShowBlockPlayer(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void ShowFriends(ref ShowFriendsOptions options, object clientData, OnShowFriendsCallback completionDelegate)
	{
		ShowFriendsOptionsInternal optionsInternal = default(ShowFriendsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnShowFriendsCallbackInternal completionDelegateInternal = OnShowFriendsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_UI_ShowFriends(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void ShowNativeProfile(ref ShowNativeProfileOptions options, object clientData, OnShowNativeProfileCallback completionDelegate)
	{
		ShowNativeProfileOptionsInternal optionsInternal = default(ShowNativeProfileOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnShowNativeProfileCallbackInternal completionDelegateInternal = OnShowNativeProfileCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_UI_ShowNativeProfile(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void ShowReportPlayer(ref ShowReportPlayerOptions options, object clientData, OnShowReportPlayerCallback completionDelegate)
	{
		ShowReportPlayerOptionsInternal optionsInternal = default(ShowReportPlayerOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnShowReportPlayerCallbackInternal completionDelegateInternal = OnShowReportPlayerCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_UI_ShowReportPlayer(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnDisplaySettingsUpdatedCallbackInternal))]
	internal static void OnDisplaySettingsUpdatedCallbackInternalImplementation(ref OnDisplaySettingsUpdatedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnDisplaySettingsUpdatedCallbackInfoInternal, OnDisplaySettingsUpdatedCallback, OnDisplaySettingsUpdatedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnHideFriendsCallbackInternal))]
	internal static void OnHideFriendsCallbackInternalImplementation(ref HideFriendsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<HideFriendsCallbackInfoInternal, OnHideFriendsCallback, HideFriendsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnMemoryMonitorCallbackInternal))]
	internal static void OnMemoryMonitorCallbackInternalImplementation(ref MemoryMonitorCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<MemoryMonitorCallbackInfoInternal, OnMemoryMonitorCallback, MemoryMonitorCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnShowBlockPlayerCallbackInternal))]
	internal static void OnShowBlockPlayerCallbackInternalImplementation(ref OnShowBlockPlayerCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnShowBlockPlayerCallbackInfoInternal, OnShowBlockPlayerCallback, OnShowBlockPlayerCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnShowFriendsCallbackInternal))]
	internal static void OnShowFriendsCallbackInternalImplementation(ref ShowFriendsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<ShowFriendsCallbackInfoInternal, OnShowFriendsCallback, ShowFriendsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnShowNativeProfileCallbackInternal))]
	internal static void OnShowNativeProfileCallbackInternalImplementation(ref ShowNativeProfileCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<ShowNativeProfileCallbackInfoInternal, OnShowNativeProfileCallback, ShowNativeProfileCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnShowReportPlayerCallbackInternal))]
	internal static void OnShowReportPlayerCallbackInternalImplementation(ref OnShowReportPlayerCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnShowReportPlayerCallbackInfoInternal, OnShowReportPlayerCallback, OnShowReportPlayerCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
