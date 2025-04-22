using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct DuplicateFileOptionsInternal : ISettable<DuplicateFileOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_SourceFilename;

	private IntPtr m_DestinationFilename;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String SourceFilename
	{
		set
		{
			Helper.Set(value, ref m_SourceFilename);
		}
	}

	public Utf8String DestinationFilename
	{
		set
		{
			Helper.Set(value, ref m_DestinationFilename);
		}
	}

	public void Set(ref DuplicateFileOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		SourceFilename = other.SourceFilename;
		DestinationFilename = other.DestinationFilename;
	}

	public void Set(ref DuplicateFileOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			SourceFilename = other.Value.SourceFilename;
			DestinationFilename = other.Value.DestinationFilename;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_SourceFilename);
		Helper.Dispose(ref m_DestinationFilename);
	}
}
