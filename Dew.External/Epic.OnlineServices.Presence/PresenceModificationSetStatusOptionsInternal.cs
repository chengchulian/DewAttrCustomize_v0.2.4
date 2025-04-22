using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct PresenceModificationSetStatusOptionsInternal : ISettable<PresenceModificationSetStatusOptions>, IDisposable
{
	private int m_ApiVersion;

	private Status m_Status;

	public Status Status
	{
		set
		{
			m_Status = value;
		}
	}

	public void Set(ref PresenceModificationSetStatusOptions other)
	{
		m_ApiVersion = 1;
		Status = other.Status;
	}

	public void Set(ref PresenceModificationSetStatusOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Status = other.Value.Status;
		}
	}

	public void Dispose()
	{
	}
}
