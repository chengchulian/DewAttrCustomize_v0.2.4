using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient;

public struct RegisterPeerOptions
{
	public IntPtr PeerHandle { get; set; }

	public AntiCheatCommonClientType ClientType { get; set; }

	public AntiCheatCommonClientPlatform ClientPlatform { get; set; }

	public uint AuthenticationTimeout { get; set; }

	public Utf8String AccountId_DEPRECATED { get; set; }

	public Utf8String IpAddress { get; set; }

	public ProductUserId PeerProductUserId { get; set; }
}
