using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct FileMetadataInternal : IGettable<FileMetadata>, ISettable<FileMetadata>, IDisposable
{
	private int m_ApiVersion;

	private uint m_FileSizeBytes;

	private IntPtr m_MD5Hash;

	private IntPtr m_Filename;

	private long m_LastModifiedTime;

	private uint m_UnencryptedDataSizeBytes;

	public uint FileSizeBytes
	{
		get
		{
			return m_FileSizeBytes;
		}
		set
		{
			m_FileSizeBytes = value;
		}
	}

	public Utf8String MD5Hash
	{
		get
		{
			Helper.Get(m_MD5Hash, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_MD5Hash);
		}
	}

	public Utf8String Filename
	{
		get
		{
			Helper.Get(m_Filename, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Filename);
		}
	}

	public DateTimeOffset? LastModifiedTime
	{
		get
		{
			Helper.Get(m_LastModifiedTime, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LastModifiedTime);
		}
	}

	public uint UnencryptedDataSizeBytes
	{
		get
		{
			return m_UnencryptedDataSizeBytes;
		}
		set
		{
			m_UnencryptedDataSizeBytes = value;
		}
	}

	public void Set(ref FileMetadata other)
	{
		m_ApiVersion = 3;
		FileSizeBytes = other.FileSizeBytes;
		MD5Hash = other.MD5Hash;
		Filename = other.Filename;
		LastModifiedTime = other.LastModifiedTime;
		UnencryptedDataSizeBytes = other.UnencryptedDataSizeBytes;
	}

	public void Set(ref FileMetadata? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			FileSizeBytes = other.Value.FileSizeBytes;
			MD5Hash = other.Value.MD5Hash;
			Filename = other.Value.Filename;
			LastModifiedTime = other.Value.LastModifiedTime;
			UnencryptedDataSizeBytes = other.Value.UnencryptedDataSizeBytes;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_MD5Hash);
		Helper.Dispose(ref m_Filename);
	}

	public void Get(out FileMetadata output)
	{
		output = default(FileMetadata);
		output.Set(ref this);
	}
}
