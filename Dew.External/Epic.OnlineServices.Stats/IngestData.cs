namespace Epic.OnlineServices.Stats;

public struct IngestData
{
	public Utf8String StatName { get; set; }

	public int IngestAmount { get; set; }

	internal void Set(ref IngestDataInternal other)
	{
		StatName = other.StatName;
		IngestAmount = other.IngestAmount;
	}
}
