namespace Epic.OnlineServices.RTCAudio;

public struct UpdateSendingVolumeOptions
{
	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }

	public float Volume { get; set; }
}
