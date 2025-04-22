namespace Epic.OnlineServices.AntiCheatClient;

public struct OnClientIntegrityViolatedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public AntiCheatClientViolationType ViolationType { get; set; }

	public Utf8String ViolationMessage { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnClientIntegrityViolatedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		ViolationType = other.ViolationType;
		ViolationMessage = other.ViolationMessage;
	}
}
