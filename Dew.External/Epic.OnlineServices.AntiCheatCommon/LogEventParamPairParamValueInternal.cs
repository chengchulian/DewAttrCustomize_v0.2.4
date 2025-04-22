using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Explicit, Pack = 8)]
internal struct LogEventParamPairParamValueInternal : IGettable<LogEventParamPairParamValue>, ISettable<LogEventParamPairParamValue>, IDisposable
{
	[FieldOffset(0)]
	private AntiCheatCommonEventParamType m_ParamValueType;

	[FieldOffset(8)]
	private IntPtr m_ClientHandle;

	[FieldOffset(8)]
	private IntPtr m_String;

	[FieldOffset(8)]
	private uint m_UInt32;

	[FieldOffset(8)]
	private int m_Int32;

	[FieldOffset(8)]
	private ulong m_UInt64;

	[FieldOffset(8)]
	private long m_Int64;

	[FieldOffset(8)]
	private Vec3fInternal m_Vec3f;

	[FieldOffset(8)]
	private QuatInternal m_Quat;

	public IntPtr? ClientHandle
	{
		get
		{
			Helper.Get(m_ClientHandle, out IntPtr? value, m_ParamValueType, AntiCheatCommonEventParamType.ClientHandle);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ClientHandle, AntiCheatCommonEventParamType.ClientHandle, ref m_ParamValueType, this);
		}
	}

	public Utf8String String
	{
		get
		{
			Helper.Get(m_String, out Utf8String value, m_ParamValueType, AntiCheatCommonEventParamType.String);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_String, AntiCheatCommonEventParamType.String, ref m_ParamValueType, this);
		}
	}

	public uint? UInt32
	{
		get
		{
			Helper.Get(m_UInt32, out uint? value, m_ParamValueType, AntiCheatCommonEventParamType.UInt32);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UInt32, AntiCheatCommonEventParamType.UInt32, ref m_ParamValueType, this);
		}
	}

	public int? Int32
	{
		get
		{
			Helper.Get(m_Int32, out int? value, m_ParamValueType, AntiCheatCommonEventParamType.Int32);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Int32, AntiCheatCommonEventParamType.Int32, ref m_ParamValueType, this);
		}
	}

	public ulong? UInt64
	{
		get
		{
			Helper.Get(m_UInt64, out ulong? value, m_ParamValueType, AntiCheatCommonEventParamType.UInt64);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_UInt64, AntiCheatCommonEventParamType.UInt64, ref m_ParamValueType, this);
		}
	}

	public long? Int64
	{
		get
		{
			Helper.Get(m_Int64, out long? value, m_ParamValueType, AntiCheatCommonEventParamType.Int64);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Int64, AntiCheatCommonEventParamType.Int64, ref m_ParamValueType, this);
		}
	}

	public Vec3f Vec3f
	{
		get
		{
			Helper.Get(ref m_Vec3f, out Vec3f value, m_ParamValueType, AntiCheatCommonEventParamType.Vector3f);
			return value;
		}
		set
		{
			Helper.Set(ref value, ref m_Vec3f, AntiCheatCommonEventParamType.Vector3f, ref m_ParamValueType, this);
		}
	}

	public Quat Quat
	{
		get
		{
			Helper.Get(ref m_Quat, out Quat value, m_ParamValueType, AntiCheatCommonEventParamType.Quat);
			return value;
		}
		set
		{
			Helper.Set(ref value, ref m_Quat, AntiCheatCommonEventParamType.Quat, ref m_ParamValueType, this);
		}
	}

	public void Set(ref LogEventParamPairParamValue other)
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

	public void Set(ref LogEventParamPairParamValue? other)
	{
		if (other.HasValue)
		{
			ClientHandle = other.Value.ClientHandle;
			String = other.Value.String;
			UInt32 = other.Value.UInt32;
			Int32 = other.Value.Int32;
			UInt64 = other.Value.UInt64;
			Int64 = other.Value.Int64;
			Vec3f = other.Value.Vec3f;
			Quat = other.Value.Quat;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientHandle, m_ParamValueType, AntiCheatCommonEventParamType.ClientHandle);
		Helper.Dispose(ref m_String, m_ParamValueType, AntiCheatCommonEventParamType.String);
		Helper.Dispose(ref m_Vec3f);
		Helper.Dispose(ref m_Quat);
	}

	public void Get(out LogEventParamPairParamValue output)
	{
		output = default(LogEventParamPairParamValue);
		output.Set(ref this);
	}
}
