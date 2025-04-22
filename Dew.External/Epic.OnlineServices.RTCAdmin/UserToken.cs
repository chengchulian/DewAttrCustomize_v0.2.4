namespace Epic.OnlineServices.RTCAdmin;

public struct UserToken
{
	public ProductUserId ProductUserId { get; set; }

	public Utf8String Token { get; set; }

	internal void Set(ref UserTokenInternal other)
	{
		ProductUserId = other.ProductUserId;
		Token = other.Token;
	}
}
