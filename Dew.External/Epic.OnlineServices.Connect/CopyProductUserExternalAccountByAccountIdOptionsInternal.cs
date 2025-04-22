using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyProductUserExternalAccountByAccountIdOptionsInternal : ISettable<CopyProductUserExternalAccountByAccountIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private IntPtr m_AccountId;

	public ProductUserId TargetUserId
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

	public void Set(ref CopyProductUserExternalAccountByAccountIdOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		AccountId = other.AccountId;
	}

	public void Set(ref CopyProductUserExternalAccountByAccountIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			AccountId = other.Value.AccountId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
		Helper.Dispose(ref m_AccountId);
	}
}
