namespace Epic.OnlineServices.Leaderboards;

public struct OnQueryLeaderboardRanksCompleteCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref OnQueryLeaderboardRanksCompleteCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
	}
}
