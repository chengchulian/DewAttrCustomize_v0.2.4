namespace Epic.OnlineServices.KWS;

public struct QueryAgeGateCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public Utf8String CountryCode { get; set; }

	public uint AgeOfConsent { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref QueryAgeGateCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		CountryCode = other.CountryCode;
		AgeOfConsent = other.AgeOfConsent;
	}
}
