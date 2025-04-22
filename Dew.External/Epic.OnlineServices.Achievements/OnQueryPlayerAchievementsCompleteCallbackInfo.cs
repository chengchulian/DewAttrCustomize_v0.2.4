namespace Epic.OnlineServices.Achievements;

public struct OnQueryPlayerAchievementsCompleteCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId UserId { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref OnQueryPlayerAchievementsCompleteCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		UserId = other.UserId;
	}
}
