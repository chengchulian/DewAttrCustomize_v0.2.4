using System;

namespace Epic.OnlineServices.P2P;

public sealed class P2PInterface : Handle
{
	public const int AcceptconnectionApiLatest = 1;

	public const int AddnotifyincomingpacketqueuefullApiLatest = 1;

	public const int AddnotifypeerconnectionclosedApiLatest = 1;

	public const int AddnotifypeerconnectionestablishedApiLatest = 1;

	public const int AddnotifypeerconnectioninterruptedApiLatest = 1;

	public const int AddnotifypeerconnectionrequestApiLatest = 1;

	public const int ClearpacketqueueApiLatest = 1;

	public const int CloseconnectionApiLatest = 1;

	public const int CloseconnectionsApiLatest = 1;

	public const int GetnattypeApiLatest = 1;

	public const int GetnextreceivedpacketsizeApiLatest = 2;

	public const int GetpacketqueueinfoApiLatest = 1;

	public const int GetportrangeApiLatest = 1;

	public const int GetrelaycontrolApiLatest = 1;

	public const int MaxConnections = 32;

	public const int MaxPacketSize = 1170;

	public const int MaxQueueSizeUnlimited = 0;

	public const int QuerynattypeApiLatest = 1;

	public const int ReceivepacketApiLatest = 2;

	public const int SendpacketApiLatest = 3;

	public const int SetpacketqueuesizeApiLatest = 1;

	public const int SetportrangeApiLatest = 1;

	public const int SetrelaycontrolApiLatest = 1;

	public const int SocketidApiLatest = 1;

	public const int SocketidSocketnameSize = 33;

	public P2PInterface()
	{
	}

	public P2PInterface(IntPtr innerHandle)
		: base(innerHandle)
	{
	}

	public Result AcceptConnection(ref AcceptConnectionOptions options)
	{
		AcceptConnectionOptionsInternal optionsInternal = default(AcceptConnectionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_P2P_AcceptConnection(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public ulong AddNotifyIncomingPacketQueueFull(ref AddNotifyIncomingPacketQueueFullOptions options, object clientData, OnIncomingPacketQueueFullCallback incomingPacketQueueFullHandler)
	{
		AddNotifyIncomingPacketQueueFullOptionsInternal optionsInternal = default(AddNotifyIncomingPacketQueueFullOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnIncomingPacketQueueFullCallbackInternal incomingPacketQueueFullHandlerInternal = OnIncomingPacketQueueFullCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, incomingPacketQueueFullHandler, incomingPacketQueueFullHandlerInternal);
		ulong funcResult = Bindings.EOS_P2P_AddNotifyIncomingPacketQueueFull(base.InnerHandle, ref optionsInternal, clientDataAddress, incomingPacketQueueFullHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyPeerConnectionClosed(ref AddNotifyPeerConnectionClosedOptions options, object clientData, OnRemoteConnectionClosedCallback connectionClosedHandler)
	{
		AddNotifyPeerConnectionClosedOptionsInternal optionsInternal = default(AddNotifyPeerConnectionClosedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnRemoteConnectionClosedCallbackInternal connectionClosedHandlerInternal = OnRemoteConnectionClosedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, connectionClosedHandler, connectionClosedHandlerInternal);
		ulong funcResult = Bindings.EOS_P2P_AddNotifyPeerConnectionClosed(base.InnerHandle, ref optionsInternal, clientDataAddress, connectionClosedHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyPeerConnectionEstablished(ref AddNotifyPeerConnectionEstablishedOptions options, object clientData, OnPeerConnectionEstablishedCallback connectionEstablishedHandler)
	{
		AddNotifyPeerConnectionEstablishedOptionsInternal optionsInternal = default(AddNotifyPeerConnectionEstablishedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnPeerConnectionEstablishedCallbackInternal connectionEstablishedHandlerInternal = OnPeerConnectionEstablishedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, connectionEstablishedHandler, connectionEstablishedHandlerInternal);
		ulong funcResult = Bindings.EOS_P2P_AddNotifyPeerConnectionEstablished(base.InnerHandle, ref optionsInternal, clientDataAddress, connectionEstablishedHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyPeerConnectionInterrupted(ref AddNotifyPeerConnectionInterruptedOptions options, object clientData, OnPeerConnectionInterruptedCallback connectionInterruptedHandler)
	{
		AddNotifyPeerConnectionInterruptedOptionsInternal optionsInternal = default(AddNotifyPeerConnectionInterruptedOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnPeerConnectionInterruptedCallbackInternal connectionInterruptedHandlerInternal = OnPeerConnectionInterruptedCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, connectionInterruptedHandler, connectionInterruptedHandlerInternal);
		ulong funcResult = Bindings.EOS_P2P_AddNotifyPeerConnectionInterrupted(base.InnerHandle, ref optionsInternal, clientDataAddress, connectionInterruptedHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public ulong AddNotifyPeerConnectionRequest(ref AddNotifyPeerConnectionRequestOptions options, object clientData, OnIncomingConnectionRequestCallback connectionRequestHandler)
	{
		AddNotifyPeerConnectionRequestOptionsInternal optionsInternal = default(AddNotifyPeerConnectionRequestOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnIncomingConnectionRequestCallbackInternal connectionRequestHandlerInternal = OnIncomingConnectionRequestCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, connectionRequestHandler, connectionRequestHandlerInternal);
		ulong funcResult = Bindings.EOS_P2P_AddNotifyPeerConnectionRequest(base.InnerHandle, ref optionsInternal, clientDataAddress, connectionRequestHandlerInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);
		return funcResult;
	}

	public Result ClearPacketQueue(ref ClearPacketQueueOptions options)
	{
		ClearPacketQueueOptionsInternal optionsInternal = default(ClearPacketQueueOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_P2P_ClearPacketQueue(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result CloseConnection(ref CloseConnectionOptions options)
	{
		CloseConnectionOptionsInternal optionsInternal = default(CloseConnectionOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_P2P_CloseConnection(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result CloseConnections(ref CloseConnectionsOptions options)
	{
		CloseConnectionsOptionsInternal optionsInternal = default(CloseConnectionsOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_P2P_CloseConnections(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetNATType(ref GetNATTypeOptions options, out NATType outNATType)
	{
		GetNATTypeOptionsInternal optionsInternal = default(GetNATTypeOptionsInternal);
		optionsInternal.Set(ref options);
		outNATType = Helper.GetDefault<NATType>();
		Result result = Bindings.EOS_P2P_GetNATType(base.InnerHandle, ref optionsInternal, ref outNATType);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetNextReceivedPacketSize(ref GetNextReceivedPacketSizeOptions options, out uint outPacketSizeBytes)
	{
		GetNextReceivedPacketSizeOptionsInternal optionsInternal = default(GetNextReceivedPacketSizeOptionsInternal);
		optionsInternal.Set(ref options);
		outPacketSizeBytes = Helper.GetDefault<uint>();
		Result result = Bindings.EOS_P2P_GetNextReceivedPacketSize(base.InnerHandle, ref optionsInternal, ref outPacketSizeBytes);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetPacketQueueInfo(ref GetPacketQueueInfoOptions options, out PacketQueueInfo outPacketQueueInfo)
	{
		GetPacketQueueInfoOptionsInternal optionsInternal = default(GetPacketQueueInfoOptionsInternal);
		optionsInternal.Set(ref options);
		PacketQueueInfoInternal outPacketQueueInfoInternal = Helper.GetDefault<PacketQueueInfoInternal>();
		Result result = Bindings.EOS_P2P_GetPacketQueueInfo(base.InnerHandle, ref optionsInternal, ref outPacketQueueInfoInternal);
		Helper.Dispose(ref optionsInternal);
		Helper.Get<PacketQueueInfoInternal, PacketQueueInfo>(ref outPacketQueueInfoInternal, out outPacketQueueInfo);
		return result;
	}

	public Result GetPortRange(ref GetPortRangeOptions options, out ushort outPort, out ushort outNumAdditionalPortsToTry)
	{
		GetPortRangeOptionsInternal optionsInternal = default(GetPortRangeOptionsInternal);
		optionsInternal.Set(ref options);
		outPort = Helper.GetDefault<ushort>();
		outNumAdditionalPortsToTry = Helper.GetDefault<ushort>();
		Result result = Bindings.EOS_P2P_GetPortRange(base.InnerHandle, ref optionsInternal, ref outPort, ref outNumAdditionalPortsToTry);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result GetRelayControl(ref GetRelayControlOptions options, out RelayControl outRelayControl)
	{
		GetRelayControlOptionsInternal optionsInternal = default(GetRelayControlOptionsInternal);
		optionsInternal.Set(ref options);
		outRelayControl = Helper.GetDefault<RelayControl>();
		Result result = Bindings.EOS_P2P_GetRelayControl(base.InnerHandle, ref optionsInternal, ref outRelayControl);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public void QueryNATType(ref QueryNATTypeOptions options, object clientData, OnQueryNATTypeCompleteCallback completionDelegate)
	{
		QueryNATTypeOptionsInternal optionsInternal = default(QueryNATTypeOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr clientDataAddress = IntPtr.Zero;
		OnQueryNATTypeCompleteCallbackInternal completionDelegateInternal = OnQueryNATTypeCompleteCallbackInternalImplementation;
		Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);
		Bindings.EOS_P2P_QueryNATType(base.InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);
		Helper.Dispose(ref optionsInternal);
	}

	public Result ReceivePacket(ref ReceivePacketOptions options, out ProductUserId outPeerId, out SocketId outSocketId, out byte outChannel, ArraySegment<byte> outData, out uint outBytesWritten)
	{
		ReceivePacketOptionsInternal optionsInternal = default(ReceivePacketOptionsInternal);
		optionsInternal.Set(ref options);
		IntPtr outPeerIdAddress = IntPtr.Zero;
		SocketIdInternal outSocketIdInternal = Helper.GetDefault<SocketIdInternal>();
		outChannel = Helper.GetDefault<byte>();
		outBytesWritten = 0u;
		IntPtr outDataAddress = Helper.AddPinnedBuffer(outData);
		Result result = Bindings.EOS_P2P_ReceivePacket(base.InnerHandle, ref optionsInternal, ref outPeerIdAddress, ref outSocketIdInternal, ref outChannel, outDataAddress, ref outBytesWritten);
		Helper.Dispose(ref optionsInternal);
		Helper.Get(outPeerIdAddress, out outPeerId);
		Helper.Get<SocketIdInternal, SocketId>(ref outSocketIdInternal, out outSocketId);
		Helper.Dispose(ref outDataAddress);
		return result;
	}

	public void RemoveNotifyIncomingPacketQueueFull(ulong notificationId)
	{
		Bindings.EOS_P2P_RemoveNotifyIncomingPacketQueueFull(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyPeerConnectionClosed(ulong notificationId)
	{
		Bindings.EOS_P2P_RemoveNotifyPeerConnectionClosed(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyPeerConnectionEstablished(ulong notificationId)
	{
		Bindings.EOS_P2P_RemoveNotifyPeerConnectionEstablished(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyPeerConnectionInterrupted(ulong notificationId)
	{
		Bindings.EOS_P2P_RemoveNotifyPeerConnectionInterrupted(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public void RemoveNotifyPeerConnectionRequest(ulong notificationId)
	{
		Bindings.EOS_P2P_RemoveNotifyPeerConnectionRequest(base.InnerHandle, notificationId);
		Helper.RemoveCallbackByNotificationId(notificationId);
	}

	public Result SendPacket(ref SendPacketOptions options)
	{
		SendPacketOptionsInternal optionsInternal = default(SendPacketOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_P2P_SendPacket(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetPacketQueueSize(ref SetPacketQueueSizeOptions options)
	{
		SetPacketQueueSizeOptionsInternal optionsInternal = default(SetPacketQueueSizeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_P2P_SetPacketQueueSize(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetPortRange(ref SetPortRangeOptions options)
	{
		SetPortRangeOptionsInternal optionsInternal = default(SetPortRangeOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_P2P_SetPortRange(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	public Result SetRelayControl(ref SetRelayControlOptions options)
	{
		SetRelayControlOptionsInternal optionsInternal = default(SetRelayControlOptionsInternal);
		optionsInternal.Set(ref options);
		Result result = Bindings.EOS_P2P_SetRelayControl(base.InnerHandle, ref optionsInternal);
		Helper.Dispose(ref optionsInternal);
		return result;
	}

	[MonoPInvokeCallback(typeof(OnIncomingConnectionRequestCallbackInternal))]
	internal static void OnIncomingConnectionRequestCallbackInternalImplementation(ref OnIncomingConnectionRequestInfoInternal data)
	{
		if (Helper.TryGetCallback<OnIncomingConnectionRequestInfoInternal, OnIncomingConnectionRequestCallback, OnIncomingConnectionRequestInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnIncomingPacketQueueFullCallbackInternal))]
	internal static void OnIncomingPacketQueueFullCallbackInternalImplementation(ref OnIncomingPacketQueueFullInfoInternal data)
	{
		if (Helper.TryGetCallback<OnIncomingPacketQueueFullInfoInternal, OnIncomingPacketQueueFullCallback, OnIncomingPacketQueueFullInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnPeerConnectionEstablishedCallbackInternal))]
	internal static void OnPeerConnectionEstablishedCallbackInternalImplementation(ref OnPeerConnectionEstablishedInfoInternal data)
	{
		if (Helper.TryGetCallback<OnPeerConnectionEstablishedInfoInternal, OnPeerConnectionEstablishedCallback, OnPeerConnectionEstablishedInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnPeerConnectionInterruptedCallbackInternal))]
	internal static void OnPeerConnectionInterruptedCallbackInternalImplementation(ref OnPeerConnectionInterruptedInfoInternal data)
	{
		if (Helper.TryGetCallback<OnPeerConnectionInterruptedInfoInternal, OnPeerConnectionInterruptedCallback, OnPeerConnectionInterruptedInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnQueryNATTypeCompleteCallbackInternal))]
	internal static void OnQueryNATTypeCompleteCallbackInternalImplementation(ref OnQueryNATTypeCompleteInfoInternal data)
	{
		if (Helper.TryGetAndRemoveCallback<OnQueryNATTypeCompleteInfoInternal, OnQueryNATTypeCompleteCallback, OnQueryNATTypeCompleteInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}

	[MonoPInvokeCallback(typeof(OnRemoteConnectionClosedCallbackInternal))]
	internal static void OnRemoteConnectionClosedCallbackInternalImplementation(ref OnRemoteConnectionClosedInfoInternal data)
	{
		if (Helper.TryGetCallback<OnRemoteConnectionClosedInfoInternal, OnRemoteConnectionClosedCallback, OnRemoteConnectionClosedInfo>(ref data, out var callback, out var callbackInfo))
		{
			callback(ref callbackInfo);
		}
	}
}
