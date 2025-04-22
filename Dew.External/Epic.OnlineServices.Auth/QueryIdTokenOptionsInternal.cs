using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryIdTokenOptionsInternal : ISettable<QueryIdTokenOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetAccountId;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public EpicAccountId TargetAccountId
	{
		set
		{
			Helper.Set(value, ref m_TargetAccountId);
		}
	}

	public void Set(ref QueryIdTokenOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		TargetAccountId = other.TargetAccountId;
	}

	public void Set(ref QueryIdTokenOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			TargetAccountId = other.Value.TargetAccountId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetAccountId);
	}
}
