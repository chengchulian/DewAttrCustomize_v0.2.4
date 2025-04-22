using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyPresenceOptionsInternal : ISettable<CopyPresenceOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public EpicAccountId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public void Set(ref CopyPresenceOptions other)
	{
		m_ApiVersion = 3;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
	}

	public void Set(ref CopyPresenceOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
	}
}
