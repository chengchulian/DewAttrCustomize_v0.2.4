namespace Epic.OnlineServices.ProgressionSnapshot;

public struct AddProgressionOptions
{
	public uint SnapshotId { get; set; }

	public Utf8String Key { get; set; }

	public Utf8String Value { get; set; }
}
