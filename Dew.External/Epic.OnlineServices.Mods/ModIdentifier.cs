namespace Epic.OnlineServices.Mods;

public struct ModIdentifier
{
	public Utf8String NamespaceId { get; set; }

	public Utf8String ItemId { get; set; }

	public Utf8String ArtifactId { get; set; }

	public Utf8String Title { get; set; }

	public Utf8String Version { get; set; }

	internal void Set(ref ModIdentifierInternal other)
	{
		NamespaceId = other.NamespaceId;
		ItemId = other.ItemId;
		ArtifactId = other.ArtifactId;
		Title = other.Title;
		Version = other.Version;
	}
}
