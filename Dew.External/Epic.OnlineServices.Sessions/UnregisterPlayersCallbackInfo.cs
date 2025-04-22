namespace Epic.OnlineServices.Sessions;

public struct UnregisterPlayersCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId[] UnregisteredPlayers { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref UnregisterPlayersCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		UnregisteredPlayers = other.UnregisteredPlayers;
	}
}
