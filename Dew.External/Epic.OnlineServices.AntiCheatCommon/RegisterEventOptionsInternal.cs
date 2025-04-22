using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RegisterEventOptionsInternal : ISettable<RegisterEventOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_EventId;

	private IntPtr m_EventName;

	private AntiCheatCommonEventType m_EventType;

	private uint m_ParamDefsCount;

	private IntPtr m_ParamDefs;

	public uint EventId
	{
		set
		{
			m_EventId = value;
		}
	}

	public Utf8String EventName
	{
		set
		{
			Helper.Set(value, ref m_EventName);
		}
	}

	public AntiCheatCommonEventType EventType
	{
		set
		{
			m_EventType = value;
		}
	}

	public RegisterEventParamDef[] ParamDefs
	{
		set
		{
			Helper.Set<RegisterEventParamDef, RegisterEventParamDefInternal>(ref value, ref m_ParamDefs, out m_ParamDefsCount);
		}
	}

	public void Set(ref RegisterEventOptions other)
	{
		m_ApiVersion = 1;
		EventId = other.EventId;
		EventName = other.EventName;
		EventType = other.EventType;
		ParamDefs = other.ParamDefs;
	}

	public void Set(ref RegisterEventOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			EventId = other.Value.EventId;
			EventName = other.Value.EventName;
			EventType = other.Value.EventType;
			ParamDefs = other.Value.ParamDefs;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_EventName);
		Helper.Dispose(ref m_ParamDefs);
	}
}
