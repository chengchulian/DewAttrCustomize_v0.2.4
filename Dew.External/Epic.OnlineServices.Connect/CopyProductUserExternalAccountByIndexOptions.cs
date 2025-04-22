namespace Epic.OnlineServices.Connect;

public struct CopyProductUserExternalAccountByIndexOptions
{
	public ProductUserId TargetUserId { get; set; }

	public uint ExternalAccountInfoIndex { get; set; }
}
