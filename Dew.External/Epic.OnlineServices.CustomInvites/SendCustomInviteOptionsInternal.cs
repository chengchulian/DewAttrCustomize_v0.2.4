using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SendCustomInviteOptionsInternal : ISettable<SendCustomInviteOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserIds;

	private uint m_TargetUserIdsCount;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public ProductUserId[] TargetUserIds
	{
		set
		{
			Helper.Set(value, ref m_TargetUserIds, out m_TargetUserIdsCount);
		}
	}

	public void Set(ref SendCustomInviteOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		TargetUserIds = other.TargetUserIds;
	}

	public void Set(ref SendCustomInviteOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			TargetUserIds = other.Value.TargetUserIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserIds);
	}
}
