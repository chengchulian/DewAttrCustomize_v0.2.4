using System;

namespace Epic.OnlineServices.Achievements;

public struct PlayerAchievement
{
	public Utf8String AchievementId { get; set; }

	public double Progress { get; set; }

	public DateTimeOffset? UnlockTime { get; set; }

	public PlayerStatInfo[] StatInfo { get; set; }

	public Utf8String DisplayName { get; set; }

	public Utf8String Description { get; set; }

	public Utf8String IconURL { get; set; }

	public Utf8String FlavorText { get; set; }

	internal void Set(ref PlayerAchievementInternal other)
	{
		AchievementId = other.AchievementId;
		Progress = other.Progress;
		UnlockTime = other.UnlockTime;
		StatInfo = other.StatInfo;
		DisplayName = other.DisplayName;
		Description = other.Description;
		IconURL = other.IconURL;
		FlavorText = other.FlavorText;
	}
}
