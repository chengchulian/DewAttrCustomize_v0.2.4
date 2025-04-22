namespace Epic.OnlineServices;

public struct PageQuery
{
	public int StartIndex { get; set; }

	public int MaxCount { get; set; }

	internal void Set(ref PageQueryInternal other)
	{
		StartIndex = other.StartIndex;
		MaxCount = other.MaxCount;
	}
}
