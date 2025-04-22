using System;
using System.Threading;
using System.Threading.Tasks;
using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using UnityEngine;

namespace EpicTransport;

public class Client : Common
{
	public SocketId socketId;

	public ProductUserId serverId;

	private TimeSpan ConnectionTimeout;

	public bool isConnecting;

	public string hostAddress = "";

	private ProductUserId hostProductId;

	private TaskCompletionSource<Task> connectedComplete;

	private CancellationTokenSource cancelToken;

	public bool Connected { get; private set; }

	public bool Error { get; private set; }

	private event Action<byte[], int> OnReceivedData;

	private event Action OnConnected;

	public event Action OnDisconnected;

	private Client(EosTransport transport)
		: base(transport)
	{
		ConnectionTimeout = TimeSpan.FromSeconds(Math.Max(1, transport.timeout));
	}

	public static Client CreateClient(EosTransport transport, string host)
	{
		Client client = new Client(transport);
		client.hostAddress = host;
		client.socketId = new SocketId
		{
			SocketName = RandomString.Generate(20)
		};
		client.OnConnected += delegate
		{
			transport.OnClientConnected();
		};
		client.OnDisconnected += delegate
		{
			transport.OnClientDisconnected();
		};
		client.OnReceivedData += delegate(byte[] data, int channel)
		{
			transport.OnClientDataReceived(new ArraySegment<byte>(data), channel);
		};
		return client;
	}

	public async void Connect(string host)
	{
		cancelToken = new CancellationTokenSource();
		try
		{
			hostProductId = ProductUserId.FromString(host);
			serverId = hostProductId;
			connectedComplete = new TaskCompletionSource<Task>();
			OnConnected += SetConnectedComplete;
			SendInternal(hostProductId, socketId, InternalMessages.CONNECT);
			Task connectedCompleteTask = connectedComplete.Task;
			if (await Task.WhenAny(connectedCompleteTask, Task.Delay(ConnectionTimeout)) != connectedCompleteTask)
			{
				Debug.LogError("Connection to " + host + " timed out.");
				OnConnected -= SetConnectedComplete;
				OnConnectionFailed(hostProductId);
			}
			OnConnected -= SetConnectedComplete;
		}
		catch (FormatException)
		{
			Debug.LogError("Connection string was not in the right format. Did you enter a ProductId?");
			Error = true;
			OnConnectionFailed(hostProductId);
		}
		catch (Exception ex2)
		{
			Debug.LogError(ex2.Message);
			Error = true;
			OnConnectionFailed(hostProductId);
		}
		finally
		{
			if (Error)
			{
				OnConnectionFailed(null);
			}
		}
	}

	public void Disconnect()
	{
		if (serverId != null)
		{
			CloseP2PSessionWithUser(serverId, socketId);
			serverId = null;
			SendInternal(hostProductId, socketId, InternalMessages.DISCONNECT);
			Dispose();
			cancelToken?.Cancel();
			WaitForClose(hostProductId, socketId);
		}
	}

	private void SetConnectedComplete()
	{
		connectedComplete.SetResult(connectedComplete.Task);
	}

	protected override void OnReceiveData(byte[] data, ProductUserId clientUserId, int channel)
	{
		if (!ignoreAllMessages)
		{
			if (clientUserId != hostProductId)
			{
				Debug.LogError("Received a message from an unknown");
			}
			else
			{
				this.OnReceivedData(data, channel);
			}
		}
	}

	protected override void OnNewConnection(ref OnIncomingConnectionRequestInfo result)
	{
		if (!ignoreAllMessages)
		{
			if (deadSockets.Contains(result.SocketId?.SocketName))
			{
				Debug.LogError("Received incoming connection request from dead socket");
			}
			else if (hostProductId == result.RemoteUserId)
			{
				AcceptConnectionOptions acceptConnectionOptions = default(AcceptConnectionOptions);
				acceptConnectionOptions.LocalUserId = EOSSDKComponent.LocalUserProductId;
				acceptConnectionOptions.RemoteUserId = result.RemoteUserId;
				acceptConnectionOptions.SocketId = result.SocketId;
				AcceptConnectionOptions acceptConnectionOptions2 = acceptConnectionOptions;
				EOSSDKComponent.GetP2PInterface().AcceptConnection(ref acceptConnectionOptions2);
			}
			else
			{
				Debug.LogError("P2P Acceptance Request from unknown host ID.");
			}
		}
	}

	protected override void OnReceiveInternalData(InternalMessages type, ProductUserId clientUserId, SocketId socketId)
	{
		if (!ignoreAllMessages)
		{
			switch (type)
			{
			case InternalMessages.ACCEPT_CONNECT:
				Connected = true;
				this.OnConnected();
				Debug.Log("Connection established.");
				break;
			case InternalMessages.DISCONNECT:
				Connected = false;
				Debug.Log("Disconnected.");
				this.OnDisconnected();
				break;
			default:
				Debug.Log("Received unknown message type");
				break;
			}
		}
	}

	public void Send(byte[] data, int channelId)
	{
		Send(hostProductId, socketId, data, (byte)channelId);
	}

	protected override void OnConnectionFailed(ProductUserId remoteId)
	{
		this.OnDisconnected();
	}

	public void EosNotInitialized()
	{
		this.OnDisconnected();
	}
}
