using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct IntegratedPlatformOptionsContainerAddOptionsInternal : ISettable<IntegratedPlatformOptionsContainerAddOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Options;

	public Options? Options
	{
		set
		{
			Helper.Set<Options, OptionsInternal>(ref value, ref m_Options);
		}
	}

	public void Set(ref IntegratedPlatformOptionsContainerAddOptions other)
	{
		m_ApiVersion = 1;
		Options = other.Options;
	}

	public void Set(ref IntegratedPlatformOptionsContainerAddOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Options = other.Value.Options;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Options);
	}
}
