namespace Epic.OnlineServices.Achievements;

public struct DefinitionV2
{
	public Utf8String AchievementId { get; set; }

	public Utf8String UnlockedDisplayName { get; set; }

	public Utf8String UnlockedDescription { get; set; }

	public Utf8String LockedDisplayName { get; set; }

	public Utf8String LockedDescription { get; set; }

	public Utf8String FlavorText { get; set; }

	public Utf8String UnlockedIconURL { get; set; }

	public Utf8String LockedIconURL { get; set; }

	public bool IsHidden { get; set; }

	public StatThresholds[] StatThresholds { get; set; }

	internal void Set(ref DefinitionV2Internal other)
	{
		AchievementId = other.AchievementId;
		UnlockedDisplayName = other.UnlockedDisplayName;
		UnlockedDescription = other.UnlockedDescription;
		LockedDisplayName = other.LockedDisplayName;
		LockedDescription = other.LockedDescription;
		FlavorText = other.FlavorText;
		UnlockedIconURL = other.UnlockedIconURL;
		LockedIconURL = other.LockedIconURL;
		IsHidden = other.IsHidden;
		StatThresholds = other.StatThresholds;
	}
}
