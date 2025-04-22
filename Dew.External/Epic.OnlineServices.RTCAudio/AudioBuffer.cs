namespace Epic.OnlineServices.RTCAudio;

public struct AudioBuffer
{
	public short[] Frames { get; set; }

	public uint SampleRate { get; set; }

	public uint Channels { get; set; }

	internal void Set(ref AudioBufferInternal other)
	{
		Frames = other.Frames;
		SampleRate = other.SampleRate;
		Channels = other.Channels;
	}
}
