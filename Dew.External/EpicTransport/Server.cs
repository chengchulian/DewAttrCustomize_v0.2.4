using System;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using Mirror;
using UnityEngine;

namespace EpicTransport;

public class Server : Common
{
	private BidirectionalDictionary<ProductUserId, int> epicToMirrorIds;

	private Dictionary<ProductUserId, SocketId> epicToSocketIds;

	private int maxConnections;

	private int nextConnectionID;

	private event Action<int> OnConnected;

	private event Action<int, byte[], int> OnReceivedData;

	private event Action<int> OnDisconnected;

	private event Action<int, Exception> OnReceivedError;

	public static Server CreateServer(EosTransport transport, int maxConnections)
	{
		Server server = new Server(transport, maxConnections);
		server.OnConnected += delegate(int id)
		{
			transport.OnServerConnected(id);
		};
		server.OnDisconnected += delegate(int id)
		{
			transport.OnServerDisconnected(id);
		};
		server.OnReceivedData += delegate(int id, byte[] data, int channel)
		{
			transport.OnServerDataReceived(id, new ArraySegment<byte>(data), channel);
		};
		server.OnReceivedError += delegate(int id, Exception exception)
		{
			transport.OnServerError(id, TransportError.Unexpected, exception.ToString());
		};
		if (!EOSSDKComponent.Initialized)
		{
			Debug.LogError("EOS not initialized.");
		}
		return server;
	}

	private Server(EosTransport transport, int maxConnections)
		: base(transport)
	{
		this.maxConnections = maxConnections;
		epicToMirrorIds = new BidirectionalDictionary<ProductUserId, int>();
		epicToSocketIds = new Dictionary<ProductUserId, SocketId>();
		nextConnectionID = 1;
	}

	protected override void OnNewConnection(ref OnIncomingConnectionRequestInfo result)
	{
		if (!ignoreAllMessages)
		{
			if (deadSockets.Contains(result.SocketId?.SocketName))
			{
				Debug.LogError("Received incoming connection request from dead socket");
				return;
			}
			AcceptConnectionOptions acceptConnectionOptions = default(AcceptConnectionOptions);
			acceptConnectionOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
			acceptConnectionOptions.RemoteUserId = result.RemoteUserId;
			acceptConnectionOptions.SocketId = result.SocketId;
			AcceptConnectionOptions acceptConnectionOptions2 = acceptConnectionOptions;
			EOSSDKComponent.GetP2PInterface().AcceptConnection(ref acceptConnectionOptions2);
		}
	}

	protected override void OnReceiveInternalData(InternalMessages type, ProductUserId clientUserId, SocketId socketId)
	{
		if (ignoreAllMessages)
		{
			return;
		}
		switch (type)
		{
		case InternalMessages.CONNECT:
		{
			if (epicToMirrorIds.Count >= maxConnections)
			{
				Debug.LogError("Reached max connections");
				SendInternal(clientUserId, socketId, InternalMessages.DISCONNECT);
				break;
			}
			SendInternal(clientUserId, socketId, InternalMessages.ACCEPT_CONNECT);
			int connectionId = nextConnectionID++;
			epicToMirrorIds.Add(clientUserId, connectionId);
			epicToSocketIds.Add(clientUserId, socketId);
			this.OnConnected(connectionId);
			clientUserId.ToString(out var clientUserIdString);
			Debug.Log($"Client with Product User ID {clientUserIdString} connected. Assigning connection id {connectionId}");
			break;
		}
		case InternalMessages.DISCONNECT:
		{
			if (epicToMirrorIds.TryGetValue(clientUserId, out var connId))
			{
				this.OnDisconnected(connId);
				epicToMirrorIds.Remove(clientUserId);
				epicToSocketIds.Remove(clientUserId);
				Debug.Log($"Client with Product User ID {clientUserId} disconnected.");
			}
			else
			{
				this.OnReceivedError(-1, new Exception("ERROR Unknown Product User ID"));
			}
			break;
		}
		default:
			Debug.Log("Received unknown message type");
			break;
		}
	}

	protected override void OnReceiveData(byte[] data, ProductUserId clientUserId, int channel)
	{
		if (!ignoreAllMessages)
		{
			if (epicToMirrorIds.TryGetValue(clientUserId, out var connectionId))
			{
				this.OnReceivedData(connectionId, data, channel);
				return;
			}
			epicToSocketIds.TryGetValue(clientUserId, out var socketId);
			CloseP2PSessionWithUser(clientUserId, socketId);
			clientUserId.ToString(out var productId);
			Debug.LogError("Data received from epic client thats not known " + productId);
			this.OnReceivedError(-1, new Exception("ERROR Unknown product ID"));
		}
	}

	public void Disconnect(int connectionId)
	{
		if (epicToMirrorIds.TryGetValue(connectionId, out var userId))
		{
			epicToSocketIds.TryGetValue(userId, out var socketId);
			SendInternal(userId, socketId, InternalMessages.DISCONNECT);
			epicToMirrorIds.Remove(userId);
			epicToSocketIds.Remove(userId);
		}
		else
		{
			Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId);
		}
	}

	public void Shutdown()
	{
		foreach (KeyValuePair<ProductUserId, int> client in epicToMirrorIds)
		{
			Disconnect(client.Value);
			epicToSocketIds.TryGetValue(client.Key, out var socketId);
			WaitForClose(client.Key, socketId);
		}
		ignoreAllMessages = true;
		ReceiveData();
		Dispose();
	}

	public void SendAll(int connectionId, byte[] data, int channelId)
	{
		if (epicToMirrorIds.TryGetValue(connectionId, out var userId))
		{
			epicToSocketIds.TryGetValue(userId, out var socketId);
			Send(userId, socketId, data, (byte)channelId);
		}
		else
		{
			Debug.LogError("Trying to send on unknown connection: " + connectionId);
			this.OnReceivedError(connectionId, new Exception("ERROR Unknown Connection"));
		}
	}

	public string ServerGetClientAddress(int connectionId)
	{
		if (epicToMirrorIds.TryGetValue(connectionId, out var userId))
		{
			userId.ToString(out var userIdString);
			return userIdString;
		}
		Debug.LogError("Trying to get info on unknown connection: " + connectionId);
		this.OnReceivedError(connectionId, new Exception("ERROR Unknown Connection"));
		return string.Empty;
	}

	protected override void OnConnectionFailed(ProductUserId remoteId)
	{
		if (!ignoreAllMessages)
		{
			int connId;
			int connectionId = (epicToMirrorIds.TryGetValue(remoteId, out connId) ? connId : nextConnectionID++);
			this.OnDisconnected(connectionId);
			Debug.LogError("Connection Failed, removing user");
			epicToMirrorIds.Remove(remoteId);
			epicToSocketIds.Remove(remoteId);
		}
	}
}
