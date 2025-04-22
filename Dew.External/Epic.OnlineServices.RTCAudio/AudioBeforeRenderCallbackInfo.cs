namespace Epic.OnlineServices.RTCAudio;

public struct AudioBeforeRenderCallbackInfo : ICallbackInfo
{
	public object ClientData { get; set; }

	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }

	public AudioBuffer? Buffer { get; set; }

	public ProductUserId ParticipantId { get; set; }

	public Result? GetResultCode()
	{
		return null;
	}

	internal void Set(ref AudioBeforeRenderCallbackInfoInternal other)
	{
		ClientData = other.ClientData;
		LocalUserId = other.LocalUserId;
		RoomName = other.RoomName;
		Buffer = other.Buffer;
		ParticipantId = other.ParticipantId;
	}
}
