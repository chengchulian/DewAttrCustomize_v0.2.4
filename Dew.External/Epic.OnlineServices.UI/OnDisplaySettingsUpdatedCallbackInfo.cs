namespace Epic.OnlineServices.UI;

public struct OnDisplaySettingsUpdatedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public bool IsVisible { get; set; }

	public bool IsExclusiveInput { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref OnDisplaySettingsUpdatedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		IsVisible = other.IsVisible;
		IsExclusiveInput = other.IsExclusiveInput;
	}
}
