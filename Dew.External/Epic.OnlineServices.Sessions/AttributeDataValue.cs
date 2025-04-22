namespace Epic.OnlineServices.Sessions;

public struct AttributeDataValue
{
	private long? m_AsInt64;

	private double? m_AsDouble;

	private bool? m_AsBool;

	private Utf8String m_AsUtf8;

	private AttributeType m_ValueType;

	public long? AsInt64
	{
		get
		{
			Helper.Get(m_AsInt64, out var value, m_ValueType, AttributeType.Int64);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AsInt64, AttributeType.Int64, ref m_ValueType);
		}
	}

	public double? AsDouble
	{
		get
		{
			Helper.Get(m_AsDouble, out var value, m_ValueType, AttributeType.Double);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AsDouble, AttributeType.Double, ref m_ValueType);
		}
	}

	public bool? AsBool
	{
		get
		{
			Helper.Get(m_AsBool, out var value, m_ValueType, AttributeType.Boolean);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AsBool, AttributeType.Boolean, ref m_ValueType);
		}
	}

	public Utf8String AsUtf8
	{
		get
		{
			Helper.Get(m_AsUtf8, out var value, m_ValueType, AttributeType.String);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AsUtf8, AttributeType.String, ref m_ValueType);
		}
	}

	public AttributeType ValueType
	{
		get
		{
			return m_ValueType;
		}
		private set
		{
			m_ValueType = value;
		}
	}

	public static implicit operator AttributeDataValue(long value)
	{
		AttributeDataValue result = default(AttributeDataValue);
		result.AsInt64 = value;
		return result;
	}

	public static implicit operator AttributeDataValue(double value)
	{
		AttributeDataValue result = default(AttributeDataValue);
		result.AsDouble = value;
		return result;
	}

	public static implicit operator AttributeDataValue(bool value)
	{
		AttributeDataValue result = default(AttributeDataValue);
		result.AsBool = value;
		return result;
	}

	public static implicit operator AttributeDataValue(Utf8String value)
	{
		AttributeDataValue result = default(AttributeDataValue);
		result.AsUtf8 = value;
		return result;
	}

	public static implicit operator AttributeDataValue(string value)
	{
		AttributeDataValue result = default(AttributeDataValue);
		result.AsUtf8 = value;
		return result;
	}

	internal void Set(ref AttributeDataValueInternal other)
	{
		AsInt64 = other.AsInt64;
		AsDouble = other.AsDouble;
		AsBool = other.AsBool;
		AsUtf8 = other.AsUtf8;
	}
}
