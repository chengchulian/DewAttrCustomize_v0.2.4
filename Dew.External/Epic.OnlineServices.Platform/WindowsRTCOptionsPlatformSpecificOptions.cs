namespace Epic.OnlineServices.Platform;

public struct WindowsRTCOptionsPlatformSpecificOptions
{
	public Utf8String XAudio29DllPath { get; set; }

	internal void Set(ref WindowsRTCOptionsPlatformSpecificOptionsInternal other)
	{
		XAudio29DllPath = other.XAudio29DllPath;
	}
}
