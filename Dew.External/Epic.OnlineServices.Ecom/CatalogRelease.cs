namespace Epic.OnlineServices.Ecom;

public struct CatalogRelease
{
	public Utf8String[] CompatibleAppIds { get; set; }

	public Utf8String[] CompatiblePlatforms { get; set; }

	public Utf8String ReleaseNote { get; set; }

	internal void Set(ref CatalogReleaseInternal other)
	{
		CompatibleAppIds = other.CompatibleAppIds;
		CompatiblePlatforms = other.CompatiblePlatforms;
		ReleaseNote = other.ReleaseNote;
	}
}
