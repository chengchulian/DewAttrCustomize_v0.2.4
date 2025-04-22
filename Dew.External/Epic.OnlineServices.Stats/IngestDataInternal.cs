using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct IngestDataInternal : IGettable<IngestData>, ISettable<IngestData>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_StatName;

	private int m_IngestAmount;

	public Utf8String StatName
	{
		get
		{
			Helper.Get(m_StatName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_StatName);
		}
	}

	public int IngestAmount
	{
		get
		{
			return m_IngestAmount;
		}
		set
		{
			m_IngestAmount = value;
		}
	}

	public void Set(ref IngestData other)
	{
		m_ApiVersion = 1;
		StatName = other.StatName;
		IngestAmount = other.IngestAmount;
	}

	public void Set(ref IngestData? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			StatName = other.Value.StatName;
			IngestAmount = other.Value.IngestAmount;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_StatName);
	}

	public void Get(out IngestData output)
	{
		output = default(IngestData);
		output.Set(ref this);
	}
}
