namespace Epic.OnlineServices.RTC;

public struct ParticipantMetadata
{
	public Utf8String Key { get; set; }

	public Utf8String Value { get; set; }

	internal void Set(ref ParticipantMetadataInternal other)
	{
		Key = other.Key;
		Value = other.Value;
	}
}
