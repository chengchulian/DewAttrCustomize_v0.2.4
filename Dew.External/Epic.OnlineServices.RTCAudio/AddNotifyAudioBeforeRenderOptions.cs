namespace Epic.OnlineServices.RTCAudio;

public struct AddNotifyAudioBeforeRenderOptions
{
	public ProductUserId LocalUserId { get; set; }

	public Utf8String RoomName { get; set; }

	public bool UnmixedAudio { get; set; }
}
