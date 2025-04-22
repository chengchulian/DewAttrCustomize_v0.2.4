namespace Epic.OnlineServices.RTC;

public struct Option
{
	public Utf8String Key { get; set; }

	public Utf8String Value { get; set; }

	internal void Set(ref OptionInternal other)
	{
		Key = other.Key;
		Value = other.Value;
	}
}
