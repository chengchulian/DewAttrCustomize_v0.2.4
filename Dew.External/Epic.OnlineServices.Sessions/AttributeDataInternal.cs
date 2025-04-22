using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct AttributeDataInternal : IGettable<AttributeData>, ISettable<AttributeData>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Key;

	private AttributeDataValueInternal m_Value;

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

	public AttributeDataValue Value
	{
		get
		{
			Helper.Get<AttributeDataValueInternal, AttributeDataValue>(ref m_Value, out var value);
			return value;
		}
		set
		{
			Helper.Set(ref value, ref m_Value);
		}
	}

	public void Set(ref AttributeData other)
	{
		m_ApiVersion = 1;
		Key = other.Key;
		Value = other.Value;
	}

	public void Set(ref AttributeData? other)
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

	public void Get(out AttributeData output)
	{
		output = default(AttributeData);
		output.Set(ref this);
	}
}
