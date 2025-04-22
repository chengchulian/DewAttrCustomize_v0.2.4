using System;

namespace Epic.OnlineServices.RTC;

[Flags]
public enum JoinRoomFlags : uint
{
	None = 0u,
	EnableEcho = 1u,
	EnableDatachannel = 4u
}
