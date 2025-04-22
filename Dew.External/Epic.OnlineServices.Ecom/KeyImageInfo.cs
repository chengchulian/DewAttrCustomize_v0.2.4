namespace Epic.OnlineServices.Ecom;

public struct KeyImageInfo
{
	public Utf8String Type { get; set; }

	public Utf8String Url { get; set; }

	public uint Width { get; set; }

	public uint Height { get; set; }

	internal void Set(ref KeyImageInfoInternal other)
	{
		Type = other.Type;
		Url = other.Url;
		Width = other.Width;
		Height = other.Height;
	}
}
