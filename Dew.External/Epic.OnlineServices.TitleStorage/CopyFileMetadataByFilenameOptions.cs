namespace Epic.OnlineServices.TitleStorage;

public struct CopyFileMetadataByFilenameOptions
{
	public ProductUserId LocalUserId { get; set; }

	public Utf8String Filename { get; set; }
}
