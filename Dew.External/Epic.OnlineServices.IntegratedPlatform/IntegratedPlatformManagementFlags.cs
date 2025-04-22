using System;

namespace Epic.OnlineServices.IntegratedPlatform;

[Flags]
public enum IntegratedPlatformManagementFlags
{
	Disabled = 1,
	LibraryManagedByApplication = 2,
	LibraryManagedBySDK = 4,
	DisablePresenceMirroring = 8,
	DisableSDKManagedSessions = 0x10,
	PreferEOSIdentity = 0x20,
	PreferIntegratedIdentity = 0x40,
	ApplicationManagedIdentityLogin = 0x80
}
