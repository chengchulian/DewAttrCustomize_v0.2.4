using System;

namespace Epic.OnlineServices.RTCAudio;

public sealed class RTCAudioInterface : Handle
{
	public const int AddnotifyaudiobeforerenderApiLatest = 1;

	public const int AddnotifyaudiobeforesendApiLatest = 1;

	public const int AddnotifyaudiodeviceschangedApiLatest = 1;

	public const int AddnotifyaudioinputstateApiLatest = 1;

	public const int AddnotifyaudiooutputstateApiLatest = 1;

	public const int AddnotifyparticipantupdatedApiLatest = 1;

	public const int AudiobufferApiLatest = 1;

	public const int AudioinputdeviceinfoApiLatest = 1;

	public const int AudiooutputdeviceinfoApiLatest = 1;

	public const int CopyinputdeviceinformationbyindexApiLatest = 1;

	public const int CopyoutputdeviceinformationbyindexApiLatest = 1;

	public const int GetaudioinputdevicebyindexApiLatest = 1;

	public const int GetaudioinputdevicescountApiLatest = 1;

	public const int GetaudiooutputdevicebyindexApiLatest = 1;

	public const int GetaudiooutputdevicescountApiLatest = 1;

	public const int GetinputdevicescountApiLatest = 1;

	public const int GetoutputdevicescountApiLatest = 1;

	public const int InputdeviceinformationApiLatest = 1;

	public const int OutputdeviceinformationApiLatest = 1;

	public const int QueryinputdevicesinformationApiLatest = 1;

	public const int QueryoutputdevicesinformationApiLatest = 1;

	public const int RegisterplatformaudiouserApiLatest = 1;

	public const int RegisterplatformuserApiLatest = 1;

	public const int SendaudioApiLatest = 1;

	public const int SetaudioinputsettingsApiLatest = 1;

	public const int SetaudiooutputsettingsApiLatest = 1;

	public const int SetinputdevicesettingsApiLatest = 1;

	public const int SetoutputdevicesettingsApiLatest = 1;

	public const int UnregisterplatformaudiouserApiLatest = 1;

	public const int UnregisterplatformuserApiLatest = 1;

	public const int UpdateparticipantvolumeApiLatest = 1;

	public const int UpdatereceivingApiLatest = 1;

	public const int UpdatereceivingvolumeApiLatest = 1;

	public const int UpdatesendingApiLatest = 1;

	public const int UpdatesendingvolumeApiLatest = 1;

	public RTCAudioInterface()
	{
	}

	public RTCAudioInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyAudioBeforeRender(ref AddNotifyAudioBeforeRenderOptions options, object clientData, OnAudioBeforeRenderCallback completionDelegate)
	{
		AddNotifyAudioBeforeRenderOptionsInternal optionsInternal = default(AddNotifyAudioBeforeRenderOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAudioBeforeRenderCallbackInternal completionDelegateInternal = OnAudioBeforeRenderCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		ulong funcResult = Bindings.EOS_RTCAudio_AddNotifyAudioBeforeRender(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyAudioBeforeSend(ref AddNotifyAudioBeforeSendOptions options, object clientData, OnAudioBeforeSendCallback completionDelegate)
	{
		AddNotifyAudioBeforeSendOptionsInternal optionsInternal = default(AddNotifyAudioBeforeSendOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAudioBeforeSendCallbackInternal completionDelegateInternal = OnAudioBeforeSendCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		ulong funcResult = Bindings.EOS_RTCAudio_AddNotifyAudioBeforeSend(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyAudioDevicesChanged(ref AddNotifyAudioDevicesChangedOptions options, object clientData, OnAudioDevicesChangedCallback completionDelegate)
	{
		AddNotifyAudioDevicesChangedOptionsInternal optionsInternal = default(AddNotifyAudioDevicesChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAudioDevicesChangedCallbackInternal completionDelegateInternal = OnAudioDevicesChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		ulong funcResult = Bindings.EOS_RTCAudio_AddNotifyAudioDevicesChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyAudioInputState(ref AddNotifyAudioInputStateOptions options, object clientData, OnAudioInputStateCallback completionDelegate)
	{
		AddNotifyAudioInputStateOptionsInternal optionsInternal = default(AddNotifyAudioInputStateOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAudioInputStateCallbackInternal completionDelegateInternal = OnAudioInputStateCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		ulong funcResult = Bindings.EOS_RTCAudio_AddNotifyAudioInputState(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyAudioOutputState(ref AddNotifyAudioOutputStateOptions options, object clientData, OnAudioOutputStateCallback completionDelegate)
	{
		AddNotifyAudioOutputStateOptionsInternal optionsInternal = default(AddNotifyAudioOutputStateOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnAudioOutputStateCallbackInternal completionDelegateInternal = OnAudioOutputStateCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		ulong funcResult = Bindings.EOS_RTCAudio_AddNotifyAudioOutputState(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyParticipantUpdated(ref AddNotifyParticipantUpdatedOptions options, object clientData, OnParticipantUpdatedCallback completionDelegate)
	{
		AddNotifyParticipantUpdatedOptionsInternal optionsInternal = default(AddNotifyParticipantUpdatedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnParticipantUpdatedCallbackInternal completionDelegateInternal = OnParticipantUpdatedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		ulong funcResult = Bindings.EOS_RTCAudio_AddNotifyParticipantUpdated(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result CopyInputDeviceInformationByIndex(ref CopyInputDeviceInformationByIndexOptions options, out InputDeviceInformation? outInputDeviceInformation)
	{
		CopyInputDeviceInformationByIndexOptionsInternal optionsInternal = default(CopyInputDeviceInformationByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outInputDeviceInformationAddress = IntPtr.Zero;
		Result result = Bindings.EOS_RTCAudio_CopyInputDeviceInformationByIndex(base.InnerHandle, ref optionsInternal, ref outInputDeviceInformationAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<InputDeviceInformationInternal, InputDeviceInformation>(outInputDeviceInformationAddress, out outInputDeviceInformation);
		if (outInputDeviceInformation.HasValue)
		{
			Bindings.EOS_RTCAudio_InputDeviceInformation_Release(outInputDeviceInformationAddress);
		}
		return result;
	}

	public Result CopyOutputDeviceInformationByIndex(ref CopyOutputDeviceInformationByIndexOptions options, out OutputDeviceInformation? outOutputDeviceInformation)
	{
		CopyOutputDeviceInformationByIndexOptionsInternal optionsInternal = default(CopyOutputDeviceInformationByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outOutputDeviceInformationAddress = IntPtr.Zero;
		Result result = Bindings.EOS_RTCAudio_CopyOutputDeviceInformationByIndex(base.InnerHandle, ref optionsInternal, ref outOutputDeviceInformationAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<OutputDeviceInformationInternal, OutputDeviceInformation>(outOutputDeviceInformationAddress, out outOutputDeviceInformation);
		if (outOutputDeviceInformation.HasValue)
		{
			Bindings.EOS_RTCAudio_OutputDeviceInformation_Release(outOutputDeviceInformationAddress);
		}
		return result;
	}

	public AudioInputDeviceInfo? GetAudioInputDeviceByIndex(ref GetAudioInputDeviceByIndexOptions options)
	{
		GetAudioInputDeviceByIndexOptionsInternal optionsInternal = default(GetAudioInputDeviceByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_RTCAudio_GetAudioInputDeviceByIndex(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<AudioInputDeviceInfoInternal, AudioInputDeviceInfo>(from, out AudioInputDeviceInfo? funcResultReturn);
		return funcResultReturn;
	}

	public uint GetAudioInputDevicesCount(ref GetAudioInputDevicesCountOptions options)
	{
		GetAudioInputDevicesCountOptionsInternal optionsInternal = default(GetAudioInputDevicesCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_RTCAudio_GetAudioInputDevicesCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public AudioOutputDeviceInfo? GetAudioOutputDeviceByIndex(ref GetAudioOutputDeviceByIndexOptions options)
	{
		GetAudioOutputDeviceByIndexOptionsInternal optionsInternal = default(GetAudioOutputDeviceByIndexOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr from = Bindings.EOS_RTCAudio_GetAudioOutputDeviceByIndex(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<AudioOutputDeviceInfoInternal, AudioOutputDeviceInfo>(from, out AudioOutputDeviceInfo? funcResultReturn);
		return funcResultReturn;
	}

	public uint GetAudioOutputDevicesCount(ref GetAudioOutputDevicesCountOptions options)
	{
		GetAudioOutputDevicesCountOptionsInternal optionsInternal = default(GetAudioOutputDevicesCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_RTCAudio_GetAudioOutputDevicesCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetInputDevicesCount(ref GetInputDevicesCountOptions options)
	{
		GetInputDevicesCountOptionsInternal optionsInternal = default(GetInputDevicesCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_RTCAudio_GetInputDevicesCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public uint GetOutputDevicesCount(ref GetOutputDevicesCountOptions options)
	{
		GetOutputDevicesCountOptionsInternal optionsInternal = default(GetOutputDevicesCountOptionsInternal);
		optionsInternal.Set(ref options);
		uint result = Bindings.EOS_RTCAudio_GetOutputDevicesCount(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryInputDevicesInformation(ref QueryInputDevicesInformationOptions options, object clientData, OnQueryInputDevicesInformationCallback completionDelegate)
	{
		QueryInputDevicesInformationOptionsInternal optionsInternal = default(QueryInputDevicesInformationOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryInputDevicesInformationCallbackInternal completionDelegateInternal = OnQueryInputDevicesInformationCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_QueryInputDevicesInformation(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void QueryOutputDevicesInformation(ref QueryOutputDevicesInformationOptions options, object clientData, OnQueryOutputDevicesInformationCallback completionDelegate)
	{
		QueryOutputDevicesInformationOptionsInternal optionsInternal = default(QueryOutputDevicesInformationOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryOutputDevicesInformationCallbackInternal completionDelegateInternal = OnQueryOutputDevicesInformationCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_QueryOutputDevicesInformation(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result RegisterPlatformAudioUser(ref RegisterPlatformAudioUserOptions options)
	{
		RegisterPlatformAudioUserOptionsInternal optionsInternal = default(RegisterPlatformAudioUserOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_RTCAudio_RegisterPlatformAudioUser(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void RegisterPlatformUser(ref RegisterPlatformUserOptions options, object clientData, OnRegisterPlatformUserCallback completionDelegate)
	{
		RegisterPlatformUserOptionsInternal optionsInternal = default(RegisterPlatformUserOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRegisterPlatformUserCallbackInternal completionDelegateInternal = OnRegisterPlatformUserCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_RegisterPlatformUser(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void RemoveNotifyAudioBeforeRender(ulong notificationId)
	{
		Bindings.EOS_RTCAudio_RemoveNotifyAudioBeforeRender(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyAudioBeforeSend(ulong notificationId)
	{
		Bindings.EOS_RTCAudio_RemoveNotifyAudioBeforeSend(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyAudioDevicesChanged(ulong notificationId)
	{
		Bindings.EOS_RTCAudio_RemoveNotifyAudioDevicesChanged(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyAudioInputState(ulong notificationId)
	{
		Bindings.EOS_RTCAudio_RemoveNotifyAudioInputState(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyAudioOutputState(ulong notificationId)
	{
		Bindings.EOS_RTCAudio_RemoveNotifyAudioOutputState(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyParticipantUpdated(ulong notificationId)
	{
		Bindings.EOS_RTCAudio_RemoveNotifyParticipantUpdated(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public Result SendAudio(ref SendAudioOptions options)
	{
		SendAudioOptionsInternal optionsInternal = default(SendAudioOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_RTCAudio_SendAudio(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetAudioInputSettings(ref SetAudioInputSettingsOptions options)
	{
		SetAudioInputSettingsOptionsInternal optionsInternal = default(SetAudioInputSettingsOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_RTCAudio_SetAudioInputSettings(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetAudioOutputSettings(ref SetAudioOutputSettingsOptions options)
	{
		SetAudioOutputSettingsOptionsInternal optionsInternal = default(SetAudioOutputSettingsOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_RTCAudio_SetAudioOutputSettings(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void SetInputDeviceSettings(ref SetInputDeviceSettingsOptions options, object clientData, OnSetInputDeviceSettingsCallback completionDelegate)
	{
		SetInputDeviceSettingsOptionsInternal optionsInternal = default(SetInputDeviceSettingsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSetInputDeviceSettingsCallbackInternal completionDelegateInternal = OnSetInputDeviceSettingsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_SetInputDeviceSettings(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void SetOutputDeviceSettings(ref SetOutputDeviceSettingsOptions options, object clientData, OnSetOutputDeviceSettingsCallback completionDelegate)
	{
		SetOutputDeviceSettingsOptionsInternal optionsInternal = default(SetOutputDeviceSettingsOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnSetOutputDeviceSettingsCallbackInternal completionDelegateInternal = OnSetOutputDeviceSettingsCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_SetOutputDeviceSettings(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result UnregisterPlatformAudioUser(ref UnregisterPlatformAudioUserOptions options)
	{
		UnregisterPlatformAudioUserOptionsInternal optionsInternal = default(UnregisterPlatformAudioUserOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_RTCAudio_UnregisterPlatformAudioUser(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void UnregisterPlatformUser(ref UnregisterPlatformUserOptions options, object clientData, OnUnregisterPlatformUserCallback completionDelegate)
	{
		UnregisterPlatformUserOptionsInternal optionsInternal = default(UnregisterPlatformUserOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUnregisterPlatformUserCallbackInternal completionDelegateInternal = OnUnregisterPlatformUserCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_UnregisterPlatformUser(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateParticipantVolume(ref UpdateParticipantVolumeOptions options, object clientData, OnUpdateParticipantVolumeCallback completionDelegate)
	{
		UpdateParticipantVolumeOptionsInternal optionsInternal = default(UpdateParticipantVolumeOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateParticipantVolumeCallbackInternal completionDelegateInternal = OnUpdateParticipantVolumeCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_UpdateParticipantVolume(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateReceiving(ref UpdateReceivingOptions options, object clientData, OnUpdateReceivingCallback completionDelegate)
	{
		UpdateReceivingOptionsInternal optionsInternal = default(UpdateReceivingOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateReceivingCallbackInternal completionDelegateInternal = OnUpdateReceivingCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_UpdateReceiving(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateReceivingVolume(ref UpdateReceivingVolumeOptions options, object clientData, OnUpdateReceivingVolumeCallback completionDelegate)
	{
		UpdateReceivingVolumeOptionsInternal optionsInternal = default(UpdateReceivingVolumeOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateReceivingVolumeCallbackInternal completionDelegateInternal = OnUpdateReceivingVolumeCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_UpdateReceivingVolume(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateSending(ref UpdateSendingOptions options, object clientData, OnUpdateSendingCallback completionDelegate)
	{
		UpdateSendingOptionsInternal optionsInternal = default(UpdateSendingOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateSendingCallbackInternal completionDelegateInternal = OnUpdateSendingCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_UpdateSending(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public void UpdateSendingVolume(ref UpdateSendingVolumeOptions options, object clientData, OnUpdateSendingVolumeCallback completionDelegate)
	{
		UpdateSendingVolumeOptionsInternal optionsInternal = default(UpdateSendingVolumeOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnUpdateSendingVolumeCallbackInternal completionDelegateInternal = OnUpdateSendingVolumeCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_RTCAudio_UpdateSendingVolume(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	[MonoPInvokeCallback(typeof(OnAudioBeforeRenderCallbackInternal))]
	internal static void OnAudioBeforeRenderCallbackInternalImplementation(ref AudioBeforeRenderCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<AudioBeforeRenderCallbackInfoInternal, OnAudioBeforeRenderCallback, AudioBeforeRenderCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnAudioBeforeSendCallbackInternal))]
	internal static void OnAudioBeforeSendCallbackInternalImplementation(ref AudioBeforeSendCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<AudioBeforeSendCallbackInfoInternal, OnAudioBeforeSendCallback, AudioBeforeSendCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnAudioDevicesChangedCallbackInternal))]
	internal static void OnAudioDevicesChangedCallbackInternalImplementation(ref AudioDevicesChangedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<AudioDevicesChangedCallbackInfoInternal, OnAudioDevicesChangedCallback, AudioDevicesChangedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnAudioInputStateCallbackInternal))]
	internal static void OnAudioInputStateCallbackInternalImplementation(ref AudioInputStateCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<AudioInputStateCallbackInfoInternal, OnAudioInputStateCallback, AudioInputStateCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnAudioOutputStateCallbackInternal))]
	internal static void OnAudioOutputStateCallbackInternalImplementation(ref AudioOutputStateCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<AudioOutputStateCallbackInfoInternal, OnAudioOutputStateCallback, AudioOutputStateCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnParticipantUpdatedCallbackInternal))]
	internal static void OnParticipantUpdatedCallbackInternalImplementation(ref ParticipantUpdatedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<ParticipantUpdatedCallbackInfoInternal, OnParticipantUpdatedCallback, ParticipantUpdatedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryInputDevicesInformationCallbackInternal))]
	internal static void OnQueryInputDevicesInformationCallbackInternalImplementation(ref OnQueryInputDevicesInformationCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryInputDevicesInformationCallbackInfoInternal, OnQueryInputDevicesInformationCallback, OnQueryInputDevicesInformationCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryOutputDevicesInformationCallbackInternal))]
	internal static void OnQueryOutputDevicesInformationCallbackInternalImplementation(ref OnQueryOutputDevicesInformationCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryOutputDevicesInformationCallbackInfoInternal, OnQueryOutputDevicesInformationCallback, OnQueryOutputDevicesInformationCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRegisterPlatformUserCallbackInternal))]
	internal static void OnRegisterPlatformUserCallbackInternalImplementation(ref OnRegisterPlatformUserCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnRegisterPlatformUserCallbackInfoInternal, OnRegisterPlatformUserCallback, OnRegisterPlatformUserCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSetInputDeviceSettingsCallbackInternal))]
	internal static void OnSetInputDeviceSettingsCallbackInternalImplementation(ref OnSetInputDeviceSettingsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnSetInputDeviceSettingsCallbackInfoInternal, OnSetInputDeviceSettingsCallback, OnSetInputDeviceSettingsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnSetOutputDeviceSettingsCallbackInternal))]
	internal static void OnSetOutputDeviceSettingsCallbackInternalImplementation(ref OnSetOutputDeviceSettingsCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnSetOutputDeviceSettingsCallbackInfoInternal, OnSetOutputDeviceSettingsCallback, OnSetOutputDeviceSettingsCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUnregisterPlatformUserCallbackInternal))]
	internal static void OnUnregisterPlatformUserCallbackInternalImplementation(ref OnUnregisterPlatformUserCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnUnregisterPlatformUserCallbackInfoInternal, OnUnregisterPlatformUserCallback, OnUnregisterPlatformUserCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateParticipantVolumeCallbackInternal))]
	internal static void OnUpdateParticipantVolumeCallbackInternalImplementation(ref UpdateParticipantVolumeCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateParticipantVolumeCallbackInfoInternal, OnUpdateParticipantVolumeCallback, UpdateParticipantVolumeCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateReceivingCallbackInternal))]
	internal static void OnUpdateReceivingCallbackInternalImplementation(ref UpdateReceivingCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateReceivingCallbackInfoInternal, OnUpdateReceivingCallback, UpdateReceivingCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateReceivingVolumeCallbackInternal))]
	internal static void OnUpdateReceivingVolumeCallbackInternalImplementation(ref UpdateReceivingVolumeCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateReceivingVolumeCallbackInfoInternal, OnUpdateReceivingVolumeCallback, UpdateReceivingVolumeCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateSendingCallbackInternal))]
	internal static void OnUpdateSendingCallbackInternalImplementation(ref UpdateSendingCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateSendingCallbackInfoInternal, OnUpdateSendingCallback, UpdateSendingCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnUpdateSendingVolumeCallbackInternal))]
	internal static void OnUpdateSendingVolumeCallbackInternalImplementation(ref UpdateSendingVolumeCallbackInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<UpdateSendingVolumeCallbackInfoInternal, OnUpdateSendingVolumeCallback, UpdateSendingVolumeCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
