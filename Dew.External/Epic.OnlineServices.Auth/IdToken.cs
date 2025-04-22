namespace Epic.OnlineServices.Auth;

public struct IdToken
{
	public EpicAccountId AccountId { get; set; }

	public Utf8String JsonWebToken { get; set; }

	internal void Set(ref IdTokenInternal other)
	{
		AccountId = other.AccountId;
		JsonWebToken = other.JsonWebToken;
	}
}
