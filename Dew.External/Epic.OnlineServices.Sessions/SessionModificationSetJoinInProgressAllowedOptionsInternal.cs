using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SessionModificationSetJoinInProgressAllowedOptionsInternal : ISettable<SessionModificationSetJoinInProgressAllowedOptions>, IDisposable
{
	private int m_ApiVersion;

	private int m_AllowJoinInProgress;

	public bool AllowJoinInProgress
	{
		set
		{
			Helper.Set(value, ref m_AllowJoinInProgress);
		}
	}

	public void Set(ref SessionModificationSetJoinInProgressAllowedOptions other)
	{
		m_ApiVersion = 1;
		AllowJoinInProgress = other.AllowJoinInProgress;
	}

	public void Set(ref SessionModificationSetJoinInProgressAllowedOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			AllowJoinInProgress = other.Value.AllowJoinInProgress;
		}
	}

	public void Dispose()
	{
	}
}
