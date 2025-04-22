namespace Epic.OnlineServices.Platform;

public struct ClientCredentials
{
	public Utf8String ClientId { get; set; }

	public Utf8String ClientSecret { get; set; }

	internal void Set(ref ClientCredentialsInternal other)
	{
		ClientId = other.ClientId;
		ClientSecret = other.ClientSecret;
	}
}
