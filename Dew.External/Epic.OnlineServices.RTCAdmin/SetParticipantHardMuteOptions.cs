namespace Epic.OnlineServices.RTCAdmin;

public struct SetParticipantHardMuteOptions
{
	public Utf8String RoomName { get; set; }

	public ProductUserId TargetUserId { get; set; }

	public bool Mute { get; set; }
}
