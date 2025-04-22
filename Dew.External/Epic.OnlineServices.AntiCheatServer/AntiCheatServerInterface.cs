using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer;

public sealed class AntiCheatServerInterface : Handle
{
	public const int AddnotifyclientactionrequiredApiLatest = 1;

	public const int AddnotifyclientauthstatuschangedApiLatest = 1;

	public const int AddnotifymessagetoclientApiLatest = 1;

	public const int BeginsessionApiLatest = 3;

	public const int BeginsessionMaxRegistertimeout = 120;

	public const int BeginsessionMinRegistertimeout = 10;

	public const int EndsessionApiLatest = 1;

	public const int GetprotectmessageoutputlengthApiLatest = 1;

	public const int ProtectmessageApiLatest = 1;

	public const int ReceivemessagefromclientApiLatest = 1;

	public const int RegisterclientApiLatest = 2;

	public const int SetclientnetworkstateApiLatest = 1;

	public const int UnprotectmessageApiLatest = 1;

	public const int UnregisterclientApiLatest = 1;

	public AntiCheatServerInterface()
	{
	}

	public AntiCheatServerInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public ulong AddNotifyClientActionRequired(ref AddNotifyClientActionRequiredOptions options, object clientData, OnClientActionRequiredCallback notificationFn)
	{
		AddNotifyClientActionRequiredOptionsInternal optionsInternal = default(AddNotifyClientActionRequiredOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnClientActionRequiredCallbackInternal notificationFnInternal = OnClientActionRequiredCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_AntiCheatServer_AddNotifyClientActionRequired(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyClientAuthStatusChanged(ref AddNotifyClientAuthStatusChangedOptions options, object clientData, OnClientAuthStatusChangedCallback notificationFn)
	{
		AddNotifyClientAuthStatusChangedOptionsInternal optionsInternal = default(AddNotifyClientAuthStatusChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnClientAuthStatusChangedCallbackInternal notificationFnInternal = OnClientAuthStatusChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_AntiCheatServer_AddNotifyClientAuthStatusChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyMessageToClient(ref AddNotifyMessageToClientOptions options, object clientData, OnMessageToClientCallback notificationFn)
	{
		AddNotifyMessageToClientOptionsInternal optionsInternal = default(AddNotifyMessageToClientOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnMessageToClientCallbackInternal notificationFnInternal = OnMessageToClientCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_AntiCheatServer_AddNotifyMessageToClient(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result BeginSession(ref BeginSessionOptions options)
	{
		BeginSessionOptionsInternal optionsInternal = default(BeginSessionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_BeginSession(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result EndSession(ref EndSessionOptions options)
	{
		EndSessionOptionsInternal optionsInternal = default(EndSessionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_EndSession(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetProtectMessageOutputLength(ref GetProtectMessageOutputLengthOptions options, out uint outBufferSizeBytes)
	{
		GetProtectMessageOutputLengthOptionsInternal optionsInternal = default(GetProtectMessageOutputLengthOptionsInternal);
		optionsInternal.Set(ref options);
		outBufferSizeBytes = Helper.GetDefault<uint>();
		Result result = Bindings.EOS_AntiCheatServer_GetProtectMessageOutputLength(base.InnerHandle, ref optionsInternal, ref outBufferSizeBytes);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogEvent(ref LogEventOptions options)
	{
		LogEventOptionsInternal optionsInternal = default(LogEventOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogEvent(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogGameRoundEnd(ref LogGameRoundEndOptions options)
	{
		LogGameRoundEndOptionsInternal optionsInternal = default(LogGameRoundEndOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogGameRoundEnd(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogGameRoundStart(ref LogGameRoundStartOptions options)
	{
		LogGameRoundStartOptionsInternal optionsInternal = default(LogGameRoundStartOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogGameRoundStart(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogPlayerDespawn(ref LogPlayerDespawnOptions options)
	{
		LogPlayerDespawnOptionsInternal optionsInternal = default(LogPlayerDespawnOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogPlayerDespawn(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogPlayerRevive(ref LogPlayerReviveOptions options)
	{
		LogPlayerReviveOptionsInternal optionsInternal = default(LogPlayerReviveOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogPlayerRevive(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogPlayerSpawn(ref LogPlayerSpawnOptions options)
	{
		LogPlayerSpawnOptionsInternal optionsInternal = default(LogPlayerSpawnOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogPlayerSpawn(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogPlayerTakeDamage(ref LogPlayerTakeDamageOptions options)
	{
		LogPlayerTakeDamageOptionsInternal optionsInternal = default(LogPlayerTakeDamageOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogPlayerTakeDamage(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogPlayerTick(ref LogPlayerTickOptions options)
	{
		LogPlayerTickOptionsInternal optionsInternal = default(LogPlayerTickOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogPlayerTick(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogPlayerUseAbility(ref LogPlayerUseAbilityOptions options)
	{
		LogPlayerUseAbilityOptionsInternal optionsInternal = default(LogPlayerUseAbilityOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogPlayerUseAbility(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result LogPlayerUseWeapon(ref LogPlayerUseWeaponOptions options)
	{
		LogPlayerUseWeaponOptionsInternal optionsInternal = default(LogPlayerUseWeaponOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_LogPlayerUseWeapon(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result ProtectMessage(ref ProtectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
	{
		ProtectMessageOptionsInternal optionsInternal = default(ProtectMessageOptionsInternal);
		optionsInternal.Set(ref options);
		outBytesWritten = 0u;
		IntPtr outBufferAddress = Helper.AddPinnedBuffer(outBuffer);
		Result result = Bindings.EOS_AntiCheatServer_ProtectMessage(base.InnerHandle, ref optionsInternal, outBufferAddress, ref outBytesWritten);
		Helper.Dispose(ref optionsInternal);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public Result ReceiveMessageFromClient(ref ReceiveMessageFromClientOptions options)
	{
		ReceiveMessageFromClientOptionsInternal optionsInternal = default(ReceiveMessageFromClientOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_ReceiveMessageFromClient(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result RegisterClient(ref RegisterClientOptions options)
	{
		RegisterClientOptionsInternal optionsInternal = default(RegisterClientOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_RegisterClient(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result RegisterEvent(ref RegisterEventOptions options)
	{
		RegisterEventOptionsInternal optionsInternal = default(RegisterEventOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_RegisterEvent(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void RemoveNotifyClientActionRequired(ulong notificationId)
	{
		Bindings.EOS_AntiCheatServer_RemoveNotifyClientActionRequired(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyClientAuthStatusChanged(ulong notificationId)
	{
		Bindings.EOS_AntiCheatServer_RemoveNotifyClientAuthStatusChanged(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyMessageToClient(ulong notificationId)
	{
		Bindings.EOS_AntiCheatServer_RemoveNotifyMessageToClient(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public Result SetClientDetails(ref SetClientDetailsOptions options)
	{
		SetClientDetailsOptionsInternal optionsInternal = default(SetClientDetailsOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_SetClientDetails(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetClientNetworkState(ref SetClientNetworkStateOptions options)
	{
		SetClientNetworkStateOptionsInternal optionsInternal = default(SetClientNetworkStateOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_SetClientNetworkState(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetGameSessionId(ref SetGameSessionIdOptions options)
	{
		SetGameSessionIdOptionsInternal optionsInternal = default(SetGameSessionIdOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_SetGameSessionId(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result UnprotectMessage(ref UnprotectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
	{
		UnprotectMessageOptionsInternal optionsInternal = default(UnprotectMessageOptionsInternal);
		optionsInternal.Set(ref options);
		outBytesWritten = 0u;
		IntPtr outBufferAddress = Helper.AddPinnedBuffer(outBuffer);
		Result result = Bindings.EOS_AntiCheatServer_UnprotectMessage(base.InnerHandle, ref optionsInternal, outBufferAddress, ref outBytesWritten);
		Helper.Dispose(ref optionsInternal);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public Result UnregisterClient(ref UnregisterClientOptions options)
	{
		UnregisterClientOptionsInternal optionsInternal = default(UnregisterClientOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatServer_UnregisterClient(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	[MonoPInvokeCallback(typeof(OnClientActionRequiredCallbackInternal))]
	internal static void OnClientActionRequiredCallbackInternalImplementation(ref OnClientActionRequiredCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnClientActionRequiredCallbackInfoInternal, OnClientActionRequiredCallback, OnClientActionRequiredCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnClientAuthStatusChangedCallbackInternal))]
	internal static void OnClientAuthStatusChangedCallbackInternalImplementation(ref OnClientAuthStatusChangedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnClientAuthStatusChangedCallbackInfoInternal, OnClientAuthStatusChangedCallback, OnClientAuthStatusChangedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnMessageToClientCallbackInternal))]
	internal static void OnMessageToClientCallbackInternalImplementation(ref OnMessageToClientCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnMessageToClientCallbackInfoInternal, OnMessageToClientCallback, OnMessageToClientCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
