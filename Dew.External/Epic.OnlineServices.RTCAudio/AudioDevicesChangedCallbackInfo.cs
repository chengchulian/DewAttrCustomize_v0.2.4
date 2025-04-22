namespace Epic.OnlineServices.RTCAudio;

public struct AudioDevicesChangedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref AudioDevicesChangedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
	}
}
