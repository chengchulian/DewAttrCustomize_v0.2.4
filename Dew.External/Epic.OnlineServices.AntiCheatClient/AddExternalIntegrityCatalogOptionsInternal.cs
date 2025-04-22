using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AddExternalIntegrityCatalogOptionsInternal : ISettable<AddExternalIntegrityCatalogOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PathToBinFile;

	public Utf8String PathToBinFile
	{
		set
		{
			Helper.Set(value, ref m_PathToBinFile);
		}
	}

	public void Set(ref AddExternalIntegrityCatalogOptions other)
	{
		m_ApiVersion = 1;
		PathToBinFile = other.PathToBinFile;
	}

	public void Set(ref AddExternalIntegrityCatalogOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PathToBinFile = other.Value.PathToBinFile;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PathToBinFile);
	}
}
