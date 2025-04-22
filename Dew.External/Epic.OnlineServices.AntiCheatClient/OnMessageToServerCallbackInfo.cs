using System;

namespace Epic.OnlineServices.AntiCheatClient;

public struct OnMessageToServerCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ArraySegment<byte> MessageData { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnMessageToServerCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		MessageData = other.MessageData;
	}
}
