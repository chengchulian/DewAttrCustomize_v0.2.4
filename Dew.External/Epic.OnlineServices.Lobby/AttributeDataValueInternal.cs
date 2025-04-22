using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby;

[StructLayout(LayoutKind.Explicit, Pack = 8)]
internal struct AttributeDataValueInternal : IGettable<AttributeDataValue>, ISettable<AttributeDataValue>, IDisposable
{
	[FieldOffset(0)]
	private long m_AsInt64;

	[FieldOffset(0)]
	private double m_AsDouble;

	[FieldOffset(0)]
	private int m_AsBool;

	[FieldOffset(0)]
	private IntPtr m_AsUtf8;

	[FieldOffset(8)]
	private AttributeType m_ValueType;

	public long? AsInt64
	{
		get
		{
			Helper.Get(m_AsInt64, out long? value, m_ValueType, AttributeType.Int64);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AsInt64, AttributeType.Int64, ref m_ValueType, this);
		}
	}

	public double? AsDouble
	{
		get
		{
			Helper.Get(m_AsDouble, out double? value, m_ValueType, AttributeType.Double);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AsDouble, AttributeType.Double, ref m_ValueType, this);
		}
	}

	public bool? AsBool
	{
		get
		{
			Helper.Get(m_AsBool, out bool? value, m_ValueType, AttributeType.Boolean);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AsBool, AttributeType.Boolean, ref m_ValueType, this);
		}
	}

	public Utf8String AsUtf8
	{
		get
		{
			Helper.Get(m_AsUtf8, out Utf8String value, m_ValueType, AttributeType.String);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AsUtf8, AttributeType.String, ref m_ValueType, this);
		}
	}

	public void Set(ref AttributeDataValue other)
	{
		AsInt64 = other.AsInt64;
		AsDouble = other.AsDouble;
		AsBool = other.AsBool;
		AsUtf8 = other.AsUtf8;
	}

	public void Set(ref AttributeDataValue? other)
	{
		if (other.HasValue)
		{
			AsInt64 = other.Value.AsInt64;
			AsDouble = other.Value.AsDouble;
			AsBool = other.Value.AsBool;
			AsUtf8 = other.Value.AsUtf8;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AsUtf8, m_ValueType, AttributeType.String);
	}

	public void Get(out AttributeDataValue output)
	{
		output = default(AttributeDataValue);
		output.Set(ref this);
	}
}
