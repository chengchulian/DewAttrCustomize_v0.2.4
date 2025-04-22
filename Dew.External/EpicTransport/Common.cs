using System;
using System.Collections;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using UnityEngine;

namespace EpicTransport;

public abstract class Common
{
	protected enum InternalMessages : byte
	{
		CONNECT,
		ACCEPT_CONNECT,
		DISCONNECT
	}

	protected struct PacketKey
	{
		public ProductUserId productUserId;

		public byte channel;
	}

	private PacketReliability[] channels;

	private byte[] internalReceiveBuffer;

	private OnIncomingConnectionRequestCallback OnIncomingConnectionRequest;

	private ulong incomingNotificationId;

	private OnRemoteConnectionClosedCallback OnRemoteConnectionClosed;

	private ulong outgoingNotificationId;

	protected readonly EosTransport transport;

	protected List<string> deadSockets;

	public bool ignoreAllMessages;

	private P2PInterface p2pInterface;

	protected Dictionary<PacketKey, List<List<Packet>>> incomingPackets;

	private int internal_ch => channels.Length;

	protected Common(EosTransport transport)
	{
		channels = transport.Channels;
		deadSockets = new List<string>();
		AddNotifyPeerConnectionRequestOptions addNotifyPeerConnectionRequestOptions = new AddNotifyPeerConnectionRequestOptions
		{
			LocalUserId = EOSSDKComponent.LocalUserProductId,
			SocketId = null
		};
		OnIncomingConnectionRequest = (OnIncomingConnectionRequestCallback)Delegate.Combine(OnIncomingConnectionRequest, new OnIncomingConnectionRequestCallback(OnNewConnection));
		OnRemoteConnectionClosed = (OnRemoteConnectionClosedCallback)Delegate.Combine(OnRemoteConnectionClosed, new OnRemoteConnectionClosedCallback(OnConnectFail));
		p2pInterface = EOSSDKComponent.GetP2PInterface();
		incomingNotificationId = p2pInterface.AddNotifyPeerConnectionRequest(ref addNotifyPeerConnectionRequestOptions, null, OnIncomingConnectionRequest);
		AddNotifyPeerConnectionClosedOptions addNotifyPeerConnectionClosedOptions = new AddNotifyPeerConnectionClosedOptions
		{
			LocalUserId = EOSSDKComponent.LocalUserProductId,
			SocketId = null
		};
		outgoingNotificationId = p2pInterface.AddNotifyPeerConnectionClosed(ref addNotifyPeerConnectionClosedOptions, null, OnRemoteConnectionClosed);
		if (outgoingNotificationId == 0L || incomingNotificationId == 0L)
		{
			Debug.LogError("Couldn't bind notifications with P2P interface");
		}
		incomingPackets = new Dictionary<PacketKey, List<List<Packet>>>();
		this.transport = transport;
		internalReceiveBuffer = new byte[1170];
	}

	protected void Dispose()
	{
		p2pInterface.RemoveNotifyPeerConnectionRequest(incomingNotificationId);
		p2pInterface.RemoveNotifyPeerConnectionClosed(outgoingNotificationId);
		transport.ResetIgnoreMessagesAtStartUpTimer();
	}

	protected abstract void OnNewConnection(ref OnIncomingConnectionRequestInfo result);

	private void OnConnectFail(ref OnRemoteConnectionClosedInfo result)
	{
		if (!ignoreAllMessages)
		{
			OnConnectionFailed(result.RemoteUserId);
			switch (result.Reason)
			{
			case ConnectionClosedReason.ClosedByLocalUser:
				Debug.Log("Connection closed: The Connection was gracefully closed by the local user.");
				break;
			case ConnectionClosedReason.ClosedByPeer:
				Debug.Log("Connection closed: The connection was gracefully closed by remote user.");
				break;
			case ConnectionClosedReason.ConnectionClosed:
				Debug.LogWarning("Connection closed: The connection was unexpectedly closed.");
				break;
			case ConnectionClosedReason.ConnectionFailed:
				Debug.LogError("Connection failed: Failed to establish connection.");
				break;
			case ConnectionClosedReason.InvalidData:
				Debug.LogError("Connection failed: The remote user sent us invalid data..");
				break;
			case ConnectionClosedReason.InvalidMessage:
				Debug.LogError("Connection failed: The remote user sent us an invalid message.");
				break;
			case ConnectionClosedReason.NegotiationFailed:
				Debug.LogError("Connection failed: Negotiation failed.");
				break;
			case ConnectionClosedReason.TimedOut:
				Debug.LogError("Connection failed: Timeout.");
				break;
			case ConnectionClosedReason.TooManyConnections:
				Debug.LogError("Connection failed: Too many connections.");
				break;
			case ConnectionClosedReason.UnexpectedError:
				Debug.LogError("Unexpected Error, connection will be closed");
				break;
			default:
				Debug.LogError("Unknown Error, connection has been closed.");
				break;
			}
		}
	}

	protected void SendInternal(ProductUserId target, SocketId socketId, InternalMessages type)
	{
		SendPacketOptions sendPacketOptions = default(SendPacketOptions);
		sendPacketOptions.AllowDelayedDelivery = true;
		sendPacketOptions.Channel = (byte)internal_ch;
		sendPacketOptions.Data = new byte[1] { (byte)type };
		sendPacketOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		sendPacketOptions.Reliability = PacketReliability.ReliableOrdered;
		sendPacketOptions.RemoteUserId = target;
		sendPacketOptions.SocketId = socketId;
		SendPacketOptions sendPacketOptions2 = sendPacketOptions;
		p2pInterface.SendPacket(ref sendPacketOptions2);
	}

	protected void Send(ProductUserId host, SocketId socketId, byte[] msgBuffer, byte channel)
	{
		SendPacketOptions sendPacketOptions = default(SendPacketOptions);
		sendPacketOptions.AllowDelayedDelivery = true;
		sendPacketOptions.Channel = channel;
		sendPacketOptions.Data = msgBuffer;
		sendPacketOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		sendPacketOptions.Reliability = channels[channel];
		sendPacketOptions.RemoteUserId = host;
		sendPacketOptions.SocketId = socketId;
		SendPacketOptions sendPacketOptions2 = sendPacketOptions;
		Result result = p2pInterface.SendPacket(ref sendPacketOptions2);
		if (result != 0)
		{
			Debug.LogError("Send failed " + result);
		}
	}

	private bool Receive(out ProductUserId clientProductUserId, out SocketId socketId, out ArraySegment<byte> receiveBuffer, byte channel)
	{
		ReceivePacketOptions receivePacketOptions = default(ReceivePacketOptions);
		receivePacketOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
		receivePacketOptions.MaxDataSizeBytes = 1170u;
		receivePacketOptions.RequestedChannel = channel;
		ReceivePacketOptions receivePacketOptions2 = receivePacketOptions;
		uint bytesWritten = 0u;
		ArraySegment<byte> outData = new ArraySegment<byte>(internalReceiveBuffer);
		Result num = p2pInterface.ReceivePacket(ref receivePacketOptions2, out clientProductUserId, out socketId, out channel, outData, out bytesWritten);
		ArraySegment<byte> arraySegment = outData;
		receiveBuffer = arraySegment[..(int)bytesWritten];
		if (num == Result.Success && receiveBuffer.Count > 0)
		{
			return true;
		}
		receiveBuffer = null;
		clientProductUserId = null;
		return false;
	}

	protected virtual void CloseP2PSessionWithUser(ProductUserId clientUserID, SocketId socketId)
	{
		if (socketId.Equals(default(SocketId)))
		{
			Debug.LogWarning("Socket ID == null | " + ignoreAllMessages);
		}
		else if (deadSockets == null)
		{
			Debug.LogWarning("DeadSockets == null");
		}
		else if (!deadSockets.Contains(socketId.SocketName))
		{
			deadSockets.Add(socketId.SocketName);
		}
	}

	protected void WaitForClose(ProductUserId clientUserID, SocketId socketId)
	{
		transport.StartCoroutine(DelayedClose(clientUserID, socketId));
	}

	private IEnumerator DelayedClose(ProductUserId clientUserID, SocketId socketId)
	{
		yield return null;
		CloseP2PSessionWithUser(clientUserID, socketId);
	}

	public void ReceiveData()
	{
		try
		{
			ProductUserId clientUserID;
			SocketId socketId;
			ArraySegment<byte> internalMessage;
			while (transport.enabled && Receive(out clientUserID, out socketId, out internalMessage, (byte)internal_ch))
			{
				if (internalMessage.Count == 1)
				{
					OnReceiveInternalData((InternalMessages)((IList<byte>)internalMessage)[0], clientUserID, socketId);
					return;
				}
				Debug.Log("Incorrect package length on internal channel.");
			}
			for (int chNum = 0; chNum < channels.Length; chNum++)
			{
				ProductUserId clientUserID2;
				ArraySegment<byte> receiveBuffer;
				while (transport.enabled && Receive(out clientUserID2, out socketId, out receiveBuffer, (byte)chNum))
				{
					PacketKey incomingPacketKey = default(PacketKey);
					incomingPacketKey.productUserId = clientUserID2;
					incomingPacketKey.channel = (byte)chNum;
					Packet packet = default(Packet);
					packet.FromBytes(receiveBuffer);
					if (!incomingPackets.ContainsKey(incomingPacketKey))
					{
						incomingPackets.Add(incomingPacketKey, new List<List<Packet>>());
					}
					List<List<Packet>> incomingPacketList = incomingPackets[incomingPacketKey];
					int packetListIndex = incomingPacketList.Count;
					for (int i = 0; i < incomingPacketList.Count; i++)
					{
						if (incomingPacketList[i][0].id == packet.id)
						{
							packetListIndex = i;
							break;
						}
					}
					if (packetListIndex == incomingPacketList.Count)
					{
						incomingPacketList.Add(new List<Packet>());
					}
					int insertionIndex = -1;
					List<Packet> incomingPacketListIndex = incomingPacketList[packetListIndex];
					for (int j = 0; j < incomingPacketListIndex.Count; j++)
					{
						if (incomingPacketListIndex[j].fragment > packet.fragment)
						{
							insertionIndex = j;
							break;
						}
					}
					if (insertionIndex >= 0)
					{
						incomingPacketListIndex.Insert(insertionIndex, packet);
					}
					else
					{
						incomingPacketListIndex.Add(packet);
					}
				}
			}
			List<List<Packet>> emptyPacketLists = new List<List<Packet>>();
			foreach (KeyValuePair<PacketKey, List<List<Packet>>> keyValuePair in incomingPackets)
			{
				foreach (List<Packet> packetList in keyValuePair.Value)
				{
					bool packetReady = true;
					int packetLength = 0;
					for (int k = 0; k < packetList.Count; k++)
					{
						Packet tempPacket = packetList[k];
						if (tempPacket.fragment != k || (k == packetList.Count - 1 && tempPacket.moreFragments))
						{
							packetReady = false;
						}
						else
						{
							packetLength += tempPacket.data.Length;
						}
					}
					if (packetReady)
					{
						byte[] data = new byte[packetLength];
						int dataIndex = 0;
						for (int l = 0; l < packetList.Count; l++)
						{
							Array.Copy(packetList[l].data, 0, data, dataIndex, packetList[l].data.Length);
							dataIndex += packetList[l].data.Length;
						}
						OnReceiveData(data, keyValuePair.Key.productUserId, keyValuePair.Key.channel);
						if (transport.ServerActive() || transport.ClientActive())
						{
							emptyPacketLists.Add(packetList);
						}
					}
				}
				foreach (List<Packet> emptyList in emptyPacketLists)
				{
					keyValuePair.Value.Remove(emptyList);
				}
				emptyPacketLists.Clear();
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	protected abstract void OnReceiveInternalData(InternalMessages type, ProductUserId clientUserID, SocketId socketId);

	protected abstract void OnReceiveData(byte[] data, ProductUserId clientUserID, int channel);

	protected abstract void OnConnectionFailed(ProductUserId remoteId);
}
