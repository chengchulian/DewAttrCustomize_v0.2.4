namespace Epic.OnlineServices.Achievements;

public struct StatThresholds
{
	public Utf8String Name { get; set; }

	public int Threshold { get; set; }

	internal void Set(ref StatThresholdsInternal other)
	{
		Name = other.Name;
		Threshold = other.Threshold;
	}
}
