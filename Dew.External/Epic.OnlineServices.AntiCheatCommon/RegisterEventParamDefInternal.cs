using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RegisterEventParamDefInternal : IGettable<RegisterEventParamDef>, ISettable<RegisterEventParamDef>, IDisposable
{
	private IntPtr m_ParamName;

	private AntiCheatCommonEventParamType m_ParamType;

	public Utf8String ParamName
	{
		get
		{
			Helper.Get(m_ParamName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ParamName);
		}
	}

	public AntiCheatCommonEventParamType ParamType
	{
		get
		{
			return m_ParamType;
		}
		set
		{
			m_ParamType = value;
		}
	}

	public void Set(ref RegisterEventParamDef other)
	{
		ParamName = other.ParamName;
		ParamType = other.ParamType;
	}

	public void Set(ref RegisterEventParamDef? other)
	{
		if (other.HasValue)
		{
			ParamName = other.Value.ParamName;
			ParamType = other.Value.ParamType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ParamName);
	}

	public void Get(out RegisterEventParamDef output)
	{
		output = default(RegisterEventParamDef);
		output.Set(ref this);
	}
}
