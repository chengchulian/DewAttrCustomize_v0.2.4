namespace IngameDebugConsole;

public static class CommandTypeExtensions
{
	public static bool IsServer(this CommandType t)
	{
		if (t != CommandType.NetworkServer && t != CommandType.GameServer)
		{
			return t == CommandType.GameServerCheat;
		}
		return true;
	}

	public static bool IsCheat(this CommandType t)
	{
		if (t != CommandType.GameCheat)
		{
			return t == CommandType.GameServerCheat;
		}
		return true;
	}

	public static bool NeedNetwork(this CommandType t)
	{
		return (int)t >= 1;
	}

	public static bool NeedGame(this CommandType t)
	{
		return (int)t >= 3;
	}
}
