namespace Epic.OnlineServices.UI;

public struct Rect
{
	public int X { get; set; }

	public int Y { get; set; }

	public uint Width { get; set; }

	public uint Height { get; set; }

	internal void Set(ref RectInternal other)
	{
		X = other.X;
		Y = other.Y;
		Width = other.Width;
		Height = other.Height;
	}
}
