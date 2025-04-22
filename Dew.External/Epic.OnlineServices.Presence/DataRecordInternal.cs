using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct DataRecordInternal : IGettable<DataRecord>, ISettable<DataRecord>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Key;

	private IntPtr m_Value;

	public Utf8String Key
	{
		get
		{
			Helper.Get(m_Key, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Key);
		}
	}

	public Utf8String Value
	{
		get
		{
			Helper.Get(m_Value, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Value);
		}
	}

	public void Set(ref DataRecord other)
	{
		m_ApiVersion = 1;
		Key = other.Key;
		Value = other.Value;
	}

	public void Set(ref DataRecord? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Key = other.Value.Key;
			Value = other.Value.Value;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Key);
		Helper.Dispose(ref m_Value);
	}

	public void Get(out DataRecord output)
	{
		output = default(DataRecord);
		output.Set(ref this);
	}
}
