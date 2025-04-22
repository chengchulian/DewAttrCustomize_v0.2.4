using System;

namespace Epic.OnlineServices.AntiCheatCommon;

public struct OnClientActionRequiredCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public IntPtr ClientHandle { get; set; }

	public AntiCheatCommonClientAction ClientAction { get; set; }

	public AntiCheatCommonClientActionReason ActionReasonCode { get; set; }

	public Utf8String ActionReasonDetailsString { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnClientActionRequiredCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		ClientHandle = other.ClientHandle;
		ClientAction = other.ClientAction;
		ActionReasonCode = other.ActionReasonCode;
		ActionReasonDetailsString = other.ActionReasonDetailsString;
	}
}
