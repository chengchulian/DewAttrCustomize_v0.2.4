using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogEventParamPairInternal : IGettable<LogEventParamPair>, ISettable<LogEventParamPair>, IDisposable
{
	private LogEventParamPairParamValueInternal m_ParamValue;

	public LogEventParamPairParamValue ParamValue
	{
		get
		{
			Helper.Get<LogEventParamPairParamValueInternal, LogEventParamPairParamValue>(ref m_ParamValue, out var value);
			return value;
		}
		set
		{
			Helper.Set(ref value, ref m_ParamValue);
		}
	}

	public void Set(ref LogEventParamPair other)
	{
		ParamValue = other.ParamValue;
	}

	public void Set(ref LogEventParamPair? other)
	{
		if (other.HasValue)
		{
			ParamValue = other.Value.ParamValue;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ParamValue);
	}

	public void Get(out LogEventParamPair output)
	{
		output = default(LogEventParamPair);
		output.Set(ref this);
	}
}
