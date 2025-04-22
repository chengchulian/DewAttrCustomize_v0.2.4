using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.IntegratedPlatform;

namespace Epic.OnlineServices.Platform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct WindowsOptionsInternal : ISettable<WindowsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Reserved;

	private IntPtr m_ProductId;

	private IntPtr m_SandboxId;

	private ClientCredentialsInternal m_ClientCredentials;

	private int m_IsServer;

	private IntPtr m_EncryptionKey;

	private IntPtr m_OverrideCountryCode;

	private IntPtr m_OverrideLocaleCode;

	private IntPtr m_DeploymentId;

	private PlatformFlags m_Flags;

	private IntPtr m_CacheDirectory;

	private uint m_TickBudgetInMilliseconds;

	private IntPtr m_RTCOptions;

	private IntPtr m_IntegratedPlatformOptionsContainerHandle;

	private IntPtr m_SystemSpecificOptions;

	public IntPtr Reserved
	{
		set
		{
			m_Reserved = value;
		}
	}

	public Utf8String ProductId
	{
		set
		{
			Helper.Set(value, ref m_ProductId);
		}
	}

	public Utf8String SandboxId
	{
		set
		{
			Helper.Set(value, ref m_SandboxId);
		}
	}

	public ClientCredentials ClientCredentials
	{
		set
		{
			Helper.Set(ref value, ref m_ClientCredentials);
		}
	}

	public bool IsServer
	{
		set
		{
			Helper.Set(value, ref m_IsServer);
		}
	}

	public Utf8String EncryptionKey
	{
		set
		{
			Helper.Set(value, ref m_EncryptionKey);
		}
	}

	public Utf8String OverrideCountryCode
	{
		set
		{
			Helper.Set(value, ref m_OverrideCountryCode);
		}
	}

	public Utf8String OverrideLocaleCode
	{
		set
		{
			Helper.Set(value, ref m_OverrideLocaleCode);
		}
	}

	public Utf8String DeploymentId
	{
		set
		{
			Helper.Set(value, ref m_DeploymentId);
		}
	}

	public PlatformFlags Flags
	{
		set
		{
			m_Flags = value;
		}
	}

	public Utf8String CacheDirectory
	{
		set
		{
			Helper.Set(value, ref m_CacheDirectory);
		}
	}

	public uint TickBudgetInMilliseconds
	{
		set
		{
			m_TickBudgetInMilliseconds = value;
		}
	}

	public WindowsRTCOptions? RTCOptions
	{
		set
		{
			Helper.Set<WindowsRTCOptions, WindowsRTCOptionsInternal>(ref value, ref m_RTCOptions);
		}
	}

	public IntegratedPlatformOptionsContainer IntegratedPlatformOptionsContainerHandle
	{
		set
		{
			Helper.Set(value, ref m_IntegratedPlatformOptionsContainerHandle);
		}
	}

	public IntPtr SystemSpecificOptions
	{
		set
		{
			m_SystemSpecificOptions = value;
		}
	}

	public void Set(ref WindowsOptions other)
	{
		m_ApiVersion = 13;
		Reserved = other.Reserved;
		ProductId = other.ProductId;
		SandboxId = other.SandboxId;
		ClientCredentials = other.ClientCredentials;
		IsServer = other.IsServer;
		EncryptionKey = other.EncryptionKey;
		OverrideCountryCode = other.OverrideCountryCode;
		OverrideLocaleCode = other.OverrideLocaleCode;
		DeploymentId = other.DeploymentId;
		Flags = other.Flags;
		CacheDirectory = other.CacheDirectory;
		TickBudgetInMilliseconds = other.TickBudgetInMilliseconds;
		RTCOptions = other.RTCOptions;
		IntegratedPlatformOptionsContainerHandle = other.IntegratedPlatformOptionsContainerHandle;
		SystemSpecificOptions = other.SystemSpecificOptions;
	}

	public void Set(ref WindowsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 13;
			Reserved = other.Value.Reserved;
			ProductId = other.Value.ProductId;
			SandboxId = other.Value.SandboxId;
			ClientCredentials = other.Value.ClientCredentials;
			IsServer = other.Value.IsServer;
			EncryptionKey = other.Value.EncryptionKey;
			OverrideCountryCode = other.Value.OverrideCountryCode;
			OverrideLocaleCode = other.Value.OverrideLocaleCode;
			DeploymentId = other.Value.DeploymentId;
			Flags = other.Value.Flags;
			CacheDirectory = other.Value.CacheDirectory;
			TickBudgetInMilliseconds = other.Value.TickBudgetInMilliseconds;
			RTCOptions = other.Value.RTCOptions;
			IntegratedPlatformOptionsContainerHandle = other.Value.IntegratedPlatformOptionsContainerHandle;
			SystemSpecificOptions = other.Value.SystemSpecificOptions;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Reserved);
		Helper.Dispose(ref m_ProductId);
		Helper.Dispose(ref m_SandboxId);
		Helper.Dispose(ref m_ClientCredentials);
		Helper.Dispose(ref m_EncryptionKey);
		Helper.Dispose(ref m_OverrideCountryCode);
		Helper.Dispose(ref m_OverrideLocaleCode);
		Helper.Dispose(ref m_DeploymentId);
		Helper.Dispose(ref m_CacheDirectory);
		Helper.Dispose(ref m_RTCOptions);
		Helper.Dispose(ref m_IntegratedPlatformOptionsContainerHandle);
		Helper.Dispose(ref m_SystemSpecificOptions);
	}
}
