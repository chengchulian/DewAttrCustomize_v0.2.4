namespace Epic.OnlineServices.Auth;

public struct PinGrantInfo
{
	public Utf8String UserCode { get; set; }

	public Utf8String VerificationURI { get; set; }

	public int ExpiresIn { get; set; }

	public Utf8String VerificationURIComplete { get; set; }

	internal void Set(ref PinGrantInfoInternal other)
	{
		UserCode = other.UserCode;
		VerificationURI = other.VerificationURI;
		ExpiresIn = other.ExpiresIn;
		VerificationURIComplete = other.VerificationURIComplete;
	}
}
