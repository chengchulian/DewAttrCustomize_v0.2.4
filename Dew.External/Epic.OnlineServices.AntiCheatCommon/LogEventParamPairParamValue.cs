using System;

namespace Epic.OnlineServices.AntiCheatCommon;

public struct LogEventParamPairParamValue
{
	private AntiCheatCommonEventParamType m_ParamValueType;

	private IntPtr? m_ClientHandle;

	private Utf8String m_String;

	private uint? m_UInt32;

	private int? m_Int32;

	private ulong? m_UInt64;

	private long? m_Int64;

	private Vec3f m_Vec3f;

	private Quat m_Quat;

	public AntiCheatCommonEventParamType ParamValueType
	{
		get
		{
			return m_ParamValueType;
		}
		private set
		{
			m_ParamValueType = value;
		}
	}

	public IntPtr? ClientHandle
	{
		get
		{
			Helper.Get(m_ClientHandle, out var value, m_ParamValueType, AntiCheatCommonEventParamType.ClientHandle);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientHandle, AntiCheatCommonEventParamType.ClientHandle, ref m_ParamValueType);
		}
	}

	public Utf8String String
	{
		get
		{
			Helper.Get(m_String, out var value, m_ParamValueType, AntiCheatCommonEventParamType.String);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_String, AntiCheatCommonEventParamType.String, ref m_ParamValueType);
		}
	}

	public uint? UInt32
	{
		get
		{
			Helper.Get(m_UInt32, out var value, m_ParamValueType, AntiCheatCommonEventParamType.UInt32);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UInt32, AntiCheatCommonEventParamType.UInt32, ref m_ParamValueType);
		}
	}

	public int? Int32
	{
		get
		{
			Helper.Get(m_Int32, out var value, m_ParamValueType, AntiCheatCommonEventParamType.Int32);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Int32, AntiCheatCommonEventParamType.Int32, ref m_ParamValueType);
		}
	}

	public ulong? UInt64
	{
		get
		{
			Helper.Get(m_UInt64, out var value, m_ParamValueType, AntiCheatCommonEventParamType.UInt64);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UInt64, AntiCheatCommonEventParamType.UInt64, ref m_ParamValueType);
		}
	}

	public long? Int64
	{
		get
		{
			Helper.Get(m_Int64, out var value, m_ParamValueType, AntiCheatCommonEventParamType.Int64);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Int64, AntiCheatCommonEventParamType.Int64, ref m_ParamValueType);
		}
	}

	public Vec3f Vec3f
	{
		get
		{
			Helper.Get(m_Vec3f, out Vec3f value, m_ParamValueType, AntiCheatCommonEventParamType.Vector3f);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Vec3f, AntiCheatCommonEventParamType.Vector3f, ref m_ParamValueType);
		}
	}

	public Quat Quat
	{
		get
		{
			Helper.Get(m_Quat, out Quat value, m_ParamValueType, AntiCheatCommonEventParamType.Quat);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Quat, AntiCheatCommonEventParamType.Quat, ref m_ParamValueType);
		}
	}

	public static implicit operator LogEventParamPairParamValue(IntPtr value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.ClientHandle = value;
		return result;
	}

	public static implicit operator LogEventParamPairParamValue(Utf8String value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.String = value;
		return result;
	}

	public static implicit operator LogEventParamPairParamValue(string value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.String = value;
		return result;
	}

	public static implicit operator LogEventParamPairParamValue(uint value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.UInt32 = value;
		return result;
	}

	public static implicit operator LogEventParamPairParamValue(int value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.Int32 = value;
		return result;
	}

	public static implicit operator LogEventParamPairParamValue(ulong value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.UInt64 = value;
		return result;
	}

	public static implicit operator LogEventParamPairParamValue(long value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.Int64 = value;
		return result;
	}

	public static implicit operator LogEventParamPairParamValue(Vec3f value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.Vec3f = value;
		return result;
	}

	public static implicit operator LogEventParamPairParamValue(Quat value)
	{
		LogEventParamPairParamValue result = default(LogEventParamPairParamValue);
		result.Quat = value;
		return result;
	}

	internal void Set(ref LogEventParamPairParamValueInternal other)
	{
		ClientHandle = other.ClientHandle;
		String = other.String;
		UInt32 = other.UInt32;
		Int32 = other.Int32;
		UInt64 = other.UInt64;
		Int64 = other.Int64;
		Vec3f = other.Vec3f;
		Quat = other.Quat;
	}
}
