namespace Epic.OnlineServices.Auth;

public struct IOSCredentials
{
	public Utf8String Id { get; set; }

	public Utf8String Token { get; set; }

	public LoginCredentialType Type { get; set; }

	public IOSCredentialsSystemAuthCredentialsOptions? SystemAuthCredentialsOptions { get; set; }

	public ExternalCredentialType ExternalType { get; set; }

	internal void Set(ref IOSCredentialsInternal other)
	{
		Id = other.Id;
		Token = other.Token;
		Type = other.Type;
		SystemAuthCredentialsOptions = other.SystemAuthCredentialsOptions;
		ExternalType = other.ExternalType;
	}
}
