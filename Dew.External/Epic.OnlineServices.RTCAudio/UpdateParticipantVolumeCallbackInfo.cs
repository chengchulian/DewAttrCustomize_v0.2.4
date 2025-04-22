namespace Epic.OnlineServices.RTCAudio;

public struct UpdateParticipantVolumeCallbackInfo : ICallbackInfo
{
	public Result ResultCode { get; set; }

	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }

	public ProductUserId ParticipantId { get; set; }

	public float Volume { get; set; }

	public Result? GetResultCode()
	{
		return ResultCode;
	}

	internal void Set(ref UpdateParticipantVolumeCallbackInfoInternal other)
	{
		ResultCode = other.ResultCode;
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		ParticipantId = other.ParticipantId;
		Volume = other.Volume;
	}
}
