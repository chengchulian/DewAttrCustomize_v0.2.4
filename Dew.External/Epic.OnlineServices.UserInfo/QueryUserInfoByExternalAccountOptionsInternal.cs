using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryUserInfoByExternalAccountOptionsInternal : ISettable<QueryUserInfoByExternalAccountOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_ExternalAccountId;

	private ExternalAccountType m_AccountType;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String ExternalAccountId
	{
		set
		{
			Helper.Set(value, ref m_ExternalAccountId);
		}
	}

	public ExternalAccountType AccountType
	{
		set
		{
			m_AccountType = value;
		}
	}

	public void Set(ref QueryUserInfoByExternalAccountOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		ExternalAccountId = other.ExternalAccountId;
		AccountType = other.AccountType;
	}

	public void Set(ref QueryUserInfoByExternalAccountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			ExternalAccountId = other.Value.ExternalAccountId;
			AccountType = other.Value.AccountType;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ExternalAccountId);
	}
}
