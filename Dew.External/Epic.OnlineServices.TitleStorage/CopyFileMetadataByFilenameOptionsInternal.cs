using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyFileMetadataByFilenameOptionsInternal : ISettable<CopyFileMetadataByFilenameOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_Filename;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String Filename
	{
		set
		{
			Helper.Set(value, ref m_Filename);
		}
	}

	public void Set(ref CopyFileMetadataByFilenameOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		Filename = other.Filename;
	}

	public void Set(ref CopyFileMetadataByFilenameOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			Filename = other.Value.Filename;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_Filename);
	}
}
