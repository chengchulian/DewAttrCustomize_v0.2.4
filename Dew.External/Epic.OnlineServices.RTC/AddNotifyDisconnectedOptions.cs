namespace Epic.OnlineServices.RTC;

public struct AddNotifyDisconnectedOptions
{
	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }
}
