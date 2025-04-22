namespace Epic.OnlineServices.Achievements;

public struct OnAchievementsUnlockedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId UserId { get; set; }

	public Utf8String[] AchievementIds { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnAchievementsUnlockedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		UserId = other.UserId;
		AchievementIds = other.AchievementIds;
	}
}
