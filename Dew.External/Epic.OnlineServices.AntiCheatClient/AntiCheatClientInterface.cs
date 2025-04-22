using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient;

public sealed class AntiCheatClientInterface : Handle
{
	public const int AddexternalintegritycatalogApiLatest = 1;

	public const int AddnotifyclientintegrityviolatedApiLatest = 1;

	public const int AddnotifymessagetopeerApiLatest = 1;

	public const int AddnotifymessagetoserverApiLatest = 1;

	public const int AddnotifypeeractionrequiredApiLatest = 1;

	public const int AddnotifypeerauthstatuschangedApiLatest = 1;

	public const int BeginsessionApiLatest = 3;

	public const int EndsessionApiLatest = 1;

	public const int GetprotectmessageoutputlengthApiLatest = 1;

	public IntPtr PeerSelf = (IntPtr)(-1);

	public const int PollstatusApiLatest = 1;

	public const int ProtectmessageApiLatest = 1;

	public const int ReceivemessagefrompeerApiLatest = 1;

	public const int ReceivemessagefromserverApiLatest = 1;

	public const int RegisterpeerApiLatest = 3;

	public const int RegisterpeerMaxAuthenticationtimeout = 120;

	public const int RegisterpeerMinAuthenticationtimeout = 40;

	public const int UnprotectmessageApiLatest = 1;

	public const int UnregisterpeerApiLatest = 1;

	public AntiCheatClientInterface()
	{
	}

	public AntiCheatClientInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result AddExternalIntegrityCatalog(ref AddExternalIntegrityCatalogOptions options)
	{
		AddExternalIntegrityCatalogOptionsInternal optionsInternal = default(AddExternalIntegrityCatalogOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatClient_AddExternalIntegrityCatalog(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public ulong AddNotifyClientIntegrityViolated(ref AddNotifyClientIntegrityViolatedOptions options, object clientData, OnClientIntegrityViolatedCallback notificationFn)
	{
		AddNotifyClientIntegrityViolatedOptionsInternal optionsInternal = default(AddNotifyClientIntegrityViolatedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnClientIntegrityViolatedCallbackInternal notificationFnInternal = OnClientIntegrityViolatedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_AntiCheatClient_AddNotifyClientIntegrityViolated(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyMessageToPeer(ref AddNotifyMessageToPeerOptions options, object clientData, OnMessageToPeerCallback notificationFn)
	{
		AddNotifyMessageToPeerOptionsInternal optionsInternal = default(AddNotifyMessageToPeerOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnMessageToPeerCallbackInternal notificationFnInternal = OnMessageToPeerCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_AntiCheatClient_AddNotifyMessageToPeer(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyMessageToServer(ref AddNotifyMessageToServerOptions options, object clientData, OnMessageToServerCallback notificationFn)
	{
		AddNotifyMessageToServerOptionsInternal optionsInternal = default(AddNotifyMessageToServerOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnMessageToServerCallbackInternal notificationFnInternal = OnMessageToServerCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_AntiCheatClient_AddNotifyMessageToServer(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyPeerActionRequired(ref AddNotifyPeerActionRequiredOptions options, object clientData, OnPeerActionRequiredCallback notificationFn)
	{
		AddNotifyPeerActionRequiredOptionsInternal optionsInternal = default(AddNotifyPeerActionRequiredOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnPeerActionRequiredCallbackInternal notificationFnInternal = OnPeerActionRequiredCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_AntiCheatClient_AddNotifyPeerActionRequired(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyPeerAuthStatusChanged(ref AddNotifyPeerAuthStatusChangedOptions options, object clientData, OnPeerAuthStatusChangedCallback notificationFn)
	{
		AddNotifyPeerAuthStatusChangedOptionsInternal optionsInternal = default(AddNotifyPeerAuthStatusChangedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnPeerAuthStatusChangedCallbackInternal notificationFnInternal = OnPeerAuthStatusChangedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, notificationFn, notificationFnInternal);
		ulong funcResult = Bindings.EOS_AntiCheatClient_AddNotifyPeerAuthStatusChanged(base.InnerHandle, ref optionsInternal, clientDataAddress, notificationFnInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result BeginSession(ref BeginSessionOptions options)
	{
		BeginSessionOptionsInternal optionsInternal = default(BeginSessionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatClient_BeginSession(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result EndSession(ref EndSessionOptions options)
	{
		EndSessionOptionsInternal optionsInternal = default(EndSessionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatClient_EndSession(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetProtectMessageOutputLength(ref GetProtectMessageOutputLengthOptions options, out uint outBufferSizeBytes)
	{
		GetProtectMessageOutputLengthOptionsInternal optionsInternal = default(GetProtectMessageOutputLengthOptionsInternal);
		optionsInternal.Set(ref options);
		outBufferSizeBytes = Helper.GetDefault<uint>();
		Result result = Bindings.EOS_AntiCheatClient_GetProtectMessageOutputLength(base.InnerHandle, ref optionsInternal, ref outBufferSizeBytes);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result PollStatus(ref PollStatusOptions options, out AntiCheatClientViolationType outViolationType, out Utf8String outMessage)
	{
		PollStatusOptionsInternal optionsInternal = default(PollStatusOptionsInternal);
		optionsInternal.Set(ref options);
		outViolationType = Helper.GetDefault<AntiCheatClientViolationType>();
		IntPtr outMessageAddress = Helper.AddAllocation(options.OutMessageLength);
		Result result = Bindings.EOS_AntiCheatClient_PollStatus(base.InnerHandle, ref optionsInternal, ref outViolationType, outMessageAddress);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outMessageAddress, out outMessage);
		Helper.Dispose(ref outMessageAddress);
		return result;
	}

	public Result ProtectMessage(ref ProtectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
	{
		ProtectMessageOptionsInternal optionsInternal = default(ProtectMessageOptionsInternal);
		optionsInternal.Set(ref options);
		outBytesWritten = 0u;
		IntPtr outBufferAddress = Helper.AddPinnedBuffer(outBuffer);
		Result result = Bindings.EOS_AntiCheatClient_ProtectMessage(base.InnerHandle, ref optionsInternal, outBufferAddress, ref outBytesWritten);
		Helper.Dispose(ref optionsInternal);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public Result ReceiveMessageFromPeer(ref ReceiveMessageFromPeerOptions options)
	{
		ReceiveMessageFromPeerOptionsInternal optionsInternal = default(ReceiveMessageFromPeerOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatClient_ReceiveMessageFromPeer(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result ReceiveMessageFromServer(ref ReceiveMessageFromServerOptions options)
	{
		ReceiveMessageFromServerOptionsInternal optionsInternal = default(ReceiveMessageFromServerOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatClient_ReceiveMessageFromServer(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result RegisterPeer(ref RegisterPeerOptions options)
	{
		RegisterPeerOptionsInternal optionsInternal = default(RegisterPeerOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatClient_RegisterPeer(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void RemoveNotifyClientIntegrityViolated(ulong notificationId)
	{
		Bindings.EOS_AntiCheatClient_RemoveNotifyClientIntegrityViolated(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyMessageToPeer(ulong notificationId)
	{
		Bindings.EOS_AntiCheatClient_RemoveNotifyMessageToPeer(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyMessageToServer(ulong notificationId)
	{
		Bindings.EOS_AntiCheatClient_RemoveNotifyMessageToServer(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyPeerActionRequired(ulong notificationId)
	{
		Bindings.EOS_AntiCheatClient_RemoveNotifyPeerActionRequired(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyPeerAuthStatusChanged(ulong notificationId)
	{
		Bindings.EOS_AntiCheatClient_RemoveNotifyPeerAuthStatusChanged(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public Result UnprotectMessage(ref UnprotectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
	{
		UnprotectMessageOptionsInternal optionsInternal = default(UnprotectMessageOptionsInternal);
		optionsInternal.Set(ref options);
		outBytesWritten = 0u;
		IntPtr outBufferAddress = Helper.AddPinnedBuffer(outBuffer);
		Result result = Bindings.EOS_AntiCheatClient_UnprotectMessage(base.InnerHandle, ref optionsInternal, outBufferAddress, ref outBytesWritten);
		Helper.Dispose(ref optionsInternal);
		Helper.Dispose(ref outBufferAddress);
		return result;
	}

	public Result UnregisterPeer(ref UnregisterPeerOptions options)
	{
		UnregisterPeerOptionsInternal optionsInternal = default(UnregisterPeerOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_AntiCheatClient_UnregisterPeer(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	[MonoPInvokeCallback(typeof(OnClientIntegrityViolatedCallbackInternal))]
	internal static void OnClientIntegrityViolatedCallbackInternalImplementation(ref OnClientIntegrityViolatedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnClientIntegrityViolatedCallbackInfoInternal, OnClientIntegrityViolatedCallback, OnClientIntegrityViolatedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnMessageToPeerCallbackInternal))]
	internal static void OnMessageToPeerCallbackInternalImplementation(ref OnMessageToClientCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnMessageToClientCallbackInfoInternal, OnMessageToPeerCallback, OnMessageToClientCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnMessageToServerCallbackInternal))]
	internal static void OnMessageToServerCallbackInternalImplementation(ref OnMessageToServerCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnMessageToServerCallbackInfoInternal, OnMessageToServerCallback, OnMessageToServerCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnPeerActionRequiredCallbackInternal))]
	internal static void OnPeerActionRequiredCallbackInternalImplementation(ref OnClientActionRequiredCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnClientActionRequiredCallbackInfoInternal, OnPeerActionRequiredCallback, OnClientActionRequiredCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnPeerAuthStatusChangedCallbackInternal))]
	internal static void OnPeerAuthStatusChangedCallbackInternalImplementation(ref OnClientAuthStatusChangedCallbackInfoInternal data)
	{
		if (Helper.TryGetCallback<OnClientAuthStatusChangedCallbackInfoInternal, OnPeerAuthStatusChangedCallback, OnClientAuthStatusChangedCallbackInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
