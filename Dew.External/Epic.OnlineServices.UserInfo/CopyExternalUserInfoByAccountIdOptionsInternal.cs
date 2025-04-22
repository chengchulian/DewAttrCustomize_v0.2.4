using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyExternalUserInfoByAccountIdOptionsInternal : ISettable<CopyExternalUserInfoByAccountIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_TargetUserId;

	private IntPtr m_AccountId;

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

	public Utf8String AccountId
	{
		set
		{
			Helper.Set(value, ref m_AccountId);
		}
	}

	public void Set(ref CopyExternalUserInfoByAccountIdOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		TargetUserId = other.TargetUserId;
		AccountId = other.AccountId;
	}

	public void Set(ref CopyExternalUserInfoByAccountIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			TargetUserId = other.Value.TargetUserId;
			AccountId = other.Value.AccountId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_AccountId);
	}
}
