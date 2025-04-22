namespace Epic.OnlineServices.Sessions;

public struct RegisterPlayersCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId[] RegisteredPlayers { get; set; }

	public ProductUserId[] SanctionedPlayers { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref RegisterPlayersCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		RegisteredPlayers = other.RegisteredPlayers;
		SanctionedPlayers = other.SanctionedPlayers;
	}
}
