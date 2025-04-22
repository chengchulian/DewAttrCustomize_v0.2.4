namespace Epic.OnlineServices.P2P;

public struct OnQueryNATTypeCompleteInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public NATType NATType { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref OnQueryNATTypeCompleteInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		NATType = other.NATType;
	}
}
