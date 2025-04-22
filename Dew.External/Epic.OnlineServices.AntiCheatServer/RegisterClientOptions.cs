using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer;

public struct RegisterClientOptions
{
	public IntPtr ClientHandle { get; set; }

	public AntiCheatCommonClientType ClientType { get; set; }

	public AntiCheatCommonClientPlatform ClientPlatform { get; set; }

	public Utf8String AccountId_DEPRECATED { get; set; }

	public Utf8String IpAddress { get; set; }

	public ProductUserId UserId { get; set; }
}
