namespace Epic.OnlineServices.Achievements;

public struct CopyPlayerAchievementByAchievementIdOptions
{
	public ProductUserId TargetUserId { get; set; }

	public Utf8String AchievementId { get; set; }

	public ProductUserId LocalUserId { get; set; }
}
