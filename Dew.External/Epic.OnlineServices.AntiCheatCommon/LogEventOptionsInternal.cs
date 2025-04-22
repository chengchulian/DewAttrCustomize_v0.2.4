using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct LogEventOptionsInternal : ISettable<LogEventOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ClientHandle;

	private uint m_EventId;

	private uint m_ParamsCount;

	private IntPtr m_Params;

	public IntPtr ClientHandle
	{
		set
		{
			m_ClientHandle = value;
		}
	}

	public uint EventId
	{
		set
		{
			m_EventId = value;
		}
	}

	public LogEventParamPair[] Params
	{
		set
		{
			Helper.Set<LogEventParamPair, LogEventParamPairInternal>(ref value, ref m_Params, out m_ParamsCount);
		}
	}

	public void Set(ref LogEventOptions other)
	{
		m_ApiVersion = 1;
		ClientHandle = other.ClientHandle;
		EventId = other.EventId;
		Params = other.Params;
	}

	public void Set(ref LogEventOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ClientHandle = other.Value.ClientHandle;
			EventId = other.Value.EventId;
			Params = other.Value.Params;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ClientHandle);
		Helper.Dispose(ref m_Params);
	}
}
