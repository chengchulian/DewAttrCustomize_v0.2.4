using System;

namespace IngameDebugConsole;

[Flags]
public enum CommandType : byte
{
	Anywhere = 0,
	Network = 1,
	NetworkServer = 2,
	Game = 3,
	GameCheat = 4,
	GameServer = 5,
	GameServerCheat = 6
}
