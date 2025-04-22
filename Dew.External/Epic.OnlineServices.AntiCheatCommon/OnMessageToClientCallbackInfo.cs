using System;

namespace Epic.OnlineServices.AntiCheatCommon;

public struct OnMessageToClientCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public IntPtr ClientHandle { get; set; }

	public ArraySegment<byte> MessageData { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnMessageToClientCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		ClientHandle = other.ClientHandle;
		MessageData = other.MessageData;
	}
}
