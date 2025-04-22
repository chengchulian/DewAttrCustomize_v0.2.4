using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyProductUserExternalAccountByAccountTypeOptionsInternal : ISettable<CopyProductUserExternalAccountByAccountTypeOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_TargetUserId;

	private ExternalAccountType m_AccountIdType;

	public ProductUserId TargetUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetUserId);
		}
	}

	public ExternalAccountType AccountIdType
	{
		set
		{
			m_AccountIdType = value;
		}
	}

	public void Set(ref CopyProductUserExternalAccountByAccountTypeOptions other)
	{
		m_ApiVersion = 1;
		TargetUserId = other.TargetUserId;
		AccountIdType = other.AccountIdType;
	}

	public void Set(ref CopyProductUserExternalAccountByAccountTypeOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			TargetUserId = other.Value.TargetUserId;
			AccountIdType = other.Value.AccountIdType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_TargetUserId);
	}
}
