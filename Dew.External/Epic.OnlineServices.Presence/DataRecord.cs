namespace Epic.OnlineServices.Presence;

public struct DataRecord
{
	public Utf8String Key { get; set; }

	public Utf8String Value { get; set; }

	internal void Set(ref DataRecordInternal other)
	{
		Key = other.Key;
		Value = other.Value;
	}
}
