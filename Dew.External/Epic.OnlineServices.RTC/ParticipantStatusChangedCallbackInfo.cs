namespace Epic.OnlineServices.RTC;

public struct ParticipantStatusChangedCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }

	public ProductUserId ParticipantId { get; set; }

	public RTCParticipantStatus ParticipantStatus { get; set; }

	public ParticipantMetadata[] ParticipantMetadata { get; set; }

	public bool ParticipantInBlocklist { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref ParticipantStatusChangedCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		ParticipantId = other.ParticipantId;
		ParticipantStatus = other.ParticipantStatus;
		ParticipantMetadata = other.ParticipantMetadata;
		ParticipantInBlocklist = other.ParticipantInBlocklist;
	}
}
