namespace Epic.OnlineServices.Lobby;

public struct LocalRTCOptions
{
	public uint Flags { get; set; }

	public bool UseManualAudioInput { get; set; }

	public bool UseManualAudioOutput { get; set; }

	public bool LocalAudioDeviceInputStartsMuted { get; set; }

	internal void Set(ref LocalRTCOptionsInternal other)
	{
		Flags = other.Flags;
		UseManualAudioInput = other.UseManualAudioInput;
		UseManualAudioOutput = other.UseManualAudioOutput;
		LocalAudioDeviceInputStartsMuted = other.LocalAudioDeviceInputStartsMuted;
	}
}
