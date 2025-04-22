namespace Epic.OnlineServices.Version;

public sealed class VersionInterface
{
	public static readonly Utf8String CompanyName = "Epic Games, Inc.";

	public static readonly Utf8String CopyrightString = "Copyright Epic Games, Inc. All Rights Reserved.";

	public const int MajorVersion = 1;

	public const int MinorVersion = 16;

	public const int PatchVersion = 1;

	public static readonly Utf8String ProductIdentifier = "Epic Online Services SDK";

	public static readonly Utf8String ProductName = "Epic Online Services SDK";

	public static Utf8String GetVersion()
	{
		Helper.Get(Bindings.EOS_GetVersion(), out Utf8String funcResultReturn);
		return funcResultReturn;
	}
}
