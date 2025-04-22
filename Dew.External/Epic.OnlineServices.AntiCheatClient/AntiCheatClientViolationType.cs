namespace Epic.OnlineServices.AntiCheatClient;

public enum AntiCheatClientViolationType
{
	Invalid,
	IntegrityCatalogNotFound,
	IntegrityCatalogError,
	IntegrityCatalogCertificateRevoked,
	IntegrityCatalogMissingMainExecutable,
	GameFileMismatch,
	RequiredGameFileNotFound,
	UnknownGameFileForbidden,
	SystemFileUntrusted,
	ForbiddenModuleLoaded,
	CorruptedMemory,
	ForbiddenToolDetected,
	InternalAntiCheatViolation,
	CorruptedNetworkMessageFlow,
	VirtualMachineNotAllowed,
	ForbiddenSystemConfiguration
}
