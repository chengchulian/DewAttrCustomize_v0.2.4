using System;

namespace Epic.OnlineServices.Achievements;

public struct OnAchievementsUnlockedCallbackV2Info : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId UserId { get; set; }

	public Utf8String AchievementId { get; set; }

	public DateTimeOffset? UnlockTime { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnAchievementsUnlockedCallbackV2InfoInternal other)
	{
		ClientData = other.ClientData;
		UserId = other.UserId;
		AchievementId = other.AchievementId;
		UnlockTime = other.UnlockTime;
	}
}
