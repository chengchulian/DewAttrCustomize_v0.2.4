namespace Epic.OnlineServices.Auth;

public struct Token
{
	public Utf8String App { get; set; }

	public Utf8String ClientId { get; set; }

	public EpicAccountId AccountId { get; set; }

	public Utf8String AccessToken { get; set; }

	public double ExpiresIn { get; set; }

	public Utf8String ExpiresAt { get; set; }

	public AuthTokenType AuthType { get; set; }

	public Utf8String RefreshToken { get; set; }

	public double RefreshExpiresIn { get; set; }

	public Utf8String RefreshExpiresAt { get; set; }

	internal void Set(ref TokenInternal other)
	{
		App = other.App;
		ClientId = other.ClientId;
		AccountId = other.AccountId;
		AccessToken = other.AccessToken;
		ExpiresIn = other.ExpiresIn;
		ExpiresAt = other.ExpiresAt;
		AuthType = other.AuthType;
		RefreshToken = other.RefreshToken;
		RefreshExpiresIn = other.RefreshExpiresIn;
		RefreshExpiresAt = other.RefreshExpiresAt;
	}
}
