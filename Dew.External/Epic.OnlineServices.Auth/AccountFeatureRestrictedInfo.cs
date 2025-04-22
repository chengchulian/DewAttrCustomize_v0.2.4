namespace Epic.OnlineServices.Auth;

public struct AccountFeatureRestrictedInfo
{
	public Utf8String VerificationURI { get; set; }

	internal void Set(ref AccountFeatureRestrictedInfoInternal other)
	{
		VerificationURI = other.VerificationURI;
	}
}
