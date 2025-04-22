using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SetPresenceOptionsInternal : ISettable<SetPresenceOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_PresenceModificationHandle;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public PresenceModification PresenceModificationHandle
	{
		set
		{
			Helper.Set(value, ref m_PresenceModificationHandle);
		}
	}

	public void Set(ref SetPresenceOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		PresenceModificationHandle = other.PresenceModificationHandle;
	}

	public void Set(ref SetPresenceOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			PresenceModificationHandle = other.Value.PresenceModificationHandle;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_PresenceModificationHandle);
	}
}
