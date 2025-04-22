using System;
using Epic.OnlineServices.IntegratedPlatform;

namespace Epic.OnlineServices.Platform;

public struct WindowsOptions
{
	public IntPtr Reserved { get; set; }

	public Utf8String ProductId { get; set; }

	public Utf8String SandboxId { get; set; }

	public ClientCredentials ClientCredentials { get; set; }

	public bool IsServer { get; set; }

	public Utf8String EncryptionKey { get; set; }

	public Utf8String OverrideCountryCode { get; set; }

	public Utf8String OverrideLocaleCode { get; set; }

	public Utf8String DeploymentId { get; set; }

	public PlatformFlags Flags { get; set; }

	public Utf8String CacheDirectory { get; set; }

	public uint TickBudgetInMilliseconds { get; set; }

	public WindowsRTCOptions? RTCOptions { get; set; }

	public IntegratedPlatformOptionsContainer IntegratedPlatformOptionsContainerHandle { get; set; }

	public IntPtr SystemSpecificOptions { get; set; }
}
