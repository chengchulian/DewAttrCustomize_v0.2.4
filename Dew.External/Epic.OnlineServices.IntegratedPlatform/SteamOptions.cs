namespace Epic.OnlineServices.IntegratedPlatform;

public struct SteamOptions
{
	public Utf8String OverrideLibraryPath { get; set; }

	public uint SteamMajorVersion { get; set; }

	public uint SteamMinorVersion { get; set; }

	internal void Set(ref SteamOptionsInternal other)
	{
		OverrideLibraryPath = other.OverrideLibraryPath;
		SteamMajorVersion = other.SteamMajorVersion;
		SteamMinorVersion = other.SteamMinorVersion;
	}
}
