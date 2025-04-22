namespace Epic.OnlineServices.Achievements;

public struct PlayerStatInfo
{
	public Utf8String Name { get; set; }

	public int CurrentValue { get; set; }

	public int ThresholdValue { get; set; }

	internal void Set(ref PlayerStatInfoInternal other)
	{
		Name = other.Name;
		CurrentValue = other.CurrentValue;
		ThresholdValue = other.ThresholdValue;
	}
}
