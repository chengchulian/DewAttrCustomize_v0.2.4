namespace Epic.OnlineServices.KWS;

public struct QueryPermissionsCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String KWSUserId { get; set; }

	public Utf8String DateOfBirth { get; set; }

	public bool IsMinor { get; set; }

	public Utf8String ParentEmail { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref QueryPermissionsCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		KWSUserId = other.KWSUserId;
		DateOfBirth = other.DateOfBirth;
		IsMinor = other.IsMinor;
		ParentEmail = other.ParentEmail;
	}
}
