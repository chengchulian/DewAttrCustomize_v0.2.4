namespace Epic.OnlineServices.Connect;

public struct UserLoginInfo
{
	public Utf8String DisplayName { get; set; }

	public Utf8String NsaIdToken { get; set; }

	internal void Set(ref UserLoginInfoInternal other)
	{
		DisplayName = other.DisplayName;
		NsaIdToken = other.NsaIdToken;
	}
}
