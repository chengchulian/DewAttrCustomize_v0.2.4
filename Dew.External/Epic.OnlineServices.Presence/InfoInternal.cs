using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct InfoInternal : IGettable<Info>, ISettable<Info>, IDisposable
{
	private int m_ApiVersion;

	private Status m_Status;

	private IntPtr m_UserId;

	private IntPtr m_ProductId;

	private IntPtr m_ProductVersion;

	private IntPtr m_Platform;

	private IntPtr m_RichText;

	private int m_RecordsCount;

	private IntPtr m_Records;

	private IntPtr m_ProductName;

	private IntPtr m_IntegratedPlatform;

	public Status Status
	{
		get
		{
			return m_Status;
		}
		set
		{
			m_Status = value;
		}
	}

	public EpicAccountId UserId
	{
		get
		{
			Helper.Get(m_UserId, out EpicAccountId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UserId);
		}
	}

	public Utf8String ProductId
	{
		get
		{
			Helper.Get(m_ProductId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ProductId);
		}
	}

	public Utf8String ProductVersion
	{
		get
		{
			Helper.Get(m_ProductVersion, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ProductVersion);
		}
	}

	public Utf8String Platform
	{
		get
		{
			Helper.Get(m_Platform, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Platform);
		}
	}

	public Utf8String RichText
	{
		get
		{
			Helper.Get(m_RichText, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_RichText);
		}
	}

	public DataRecord[] Records
	{
		get
		{
			Helper.Get<DataRecordInternal, DataRecord>(m_Records, out var value, m_RecordsCount);
			return value;
		}
		set
		{
			Helper.Set<DataRecord, DataRecordInternal>(ref value, ref m_Records, out m_RecordsCount);
		}
	}

	public Utf8String ProductName
	{
		get
		{
			Helper.Get(m_ProductName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ProductName);
		}
	}

	public Utf8String IntegratedPlatform
	{
		get
		{
			Helper.Get(m_IntegratedPlatform, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_IntegratedPlatform);
		}
	}

	public void Set(ref Info other)
	{
		m_ApiVersion = 3;
		Status = other.Status;
		UserId = other.UserId;
		ProductId = other.ProductId;
		ProductVersion = other.ProductVersion;
		Platform = other.Platform;
		RichText = other.RichText;
		Records = other.Records;
		ProductName = other.ProductName;
		IntegratedPlatform = other.IntegratedPlatform;
	}

	public void Set(ref Info? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			Status = other.Value.Status;
			UserId = other.Value.UserId;
			ProductId = other.Value.ProductId;
			ProductVersion = other.Value.ProductVersion;
			Platform = other.Value.Platform;
			RichText = other.Value.RichText;
			Records = other.Value.Records;
			ProductName = other.Value.ProductName;
			IntegratedPlatform = other.Value.IntegratedPlatform;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_UserId);
		Helper.Dispose(ref m_ProductId);
		Helper.Dispose(ref m_ProductVersion);
		Helper.Dispose(ref m_Platform);
		Helper.Dispose(ref m_RichText);
		Helper.Dispose(ref m_Records);
		Helper.Dispose(ref m_ProductName);
		Helper.Dispose(ref m_IntegratedPlatform);
	}

	public void Get(out Info output)
	{
		output = default(Info);
		output.Set(ref this);
	}
}
