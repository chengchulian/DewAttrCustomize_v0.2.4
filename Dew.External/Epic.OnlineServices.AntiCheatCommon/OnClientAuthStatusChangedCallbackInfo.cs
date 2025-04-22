using System;

namespace Epic.OnlineServices.AntiCheatCommon;

public struct OnClientAuthStatusChangedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public IntPtr ClientHandle { get; set; }

	public AntiCheatCommonClientAuthStatus ClientAuthStatus { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnClientAuthStatusChangedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		ClientHandle = other.ClientHandle;
		ClientAuthStatus = other.ClientAuthStatus;
	}
}
