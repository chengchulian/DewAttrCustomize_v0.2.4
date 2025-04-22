namespace Epic.OnlineServices.Sessions;

public struct AttributeData
{
	public Utf8String Key { get; set; }

	public AttributeDataValue Value { get; set; }

	internal void Set(ref AttributeDataInternal other)
	{
		Key = other.Key;
		Value = other.Value;
	}
}
