namespace Epic.OnlineServices.AntiCheatCommon;

public struct LogEventParamPair
{
	public LogEventParamPairParamValue ParamValue { get; set; }

	internal void Set(ref LogEventParamPairInternal other)
	{
		ParamValue = other.ParamValue;
	}
}
