namespace Epic.OnlineServices.RTC;

public struct LeaveRoomOptions
{
	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }
}
