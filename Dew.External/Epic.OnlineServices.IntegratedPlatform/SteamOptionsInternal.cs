using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SteamOptionsInternal : IGettable<SteamOptions>, ISettable<SteamOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_OverrideLibraryPath;

	private uint m_SteamMajorVersion;

	private uint m_SteamMinorVersion;

	public Utf8String OverrideLibraryPath
	{
		get
		{
			Helper.Get(m_OverrideLibraryPath, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OverrideLibraryPath);
		}
	}

	public uint SteamMajorVersion
	{
		get
		{
			return m_SteamMajorVersion;
		}
		set
		{
			m_SteamMajorVersion = value;
		}
	}

	public uint SteamMinorVersion
	{
		get
		{
			return m_SteamMinorVersion;
		}
		set
		{
			m_SteamMinorVersion = value;
		}
	}

	public void Set(ref SteamOptions other)
	{
		m_ApiVersion = 2;
		OverrideLibraryPath = other.OverrideLibraryPath;
		SteamMajorVersion = other.SteamMajorVersion;
		SteamMinorVersion = other.SteamMinorVersion;
	}

	public void Set(ref SteamOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			OverrideLibraryPath = other.Value.OverrideLibraryPath;
			SteamMajorVersion = other.Value.SteamMajorVersion;
			SteamMinorVersion = other.Value.SteamMinorVersion;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_OverrideLibraryPath);
	}

	public void Get(out SteamOptions output)
	{
		output = default(SteamOptions);
		output.Set(ref this);
	}
}
