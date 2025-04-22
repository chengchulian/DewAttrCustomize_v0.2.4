namespace Epic.OnlineServices.Connect;

public struct AuthExpirationCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref AuthExpirationCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
	}
}
