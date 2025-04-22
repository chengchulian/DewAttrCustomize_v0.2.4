using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ExternalUserInfoInternal : IGettable<ExternalUserInfo>, ISettable<ExternalUserInfo>, IDisposable
{
	private int m_ApiVersion;

	private ExternalAccountType m_AccountType;

	private IntPtr m_AccountId;

	private IntPtr m_DisplayName;

	private IntPtr m_DisplayNameSanitized;

	public ExternalAccountType AccountType
	{
		get
		{
			return m_AccountType;
		}
		set
		{
			m_AccountType = value;
		}
	}

	public Utf8String AccountId
	{
		get
		{
			Helper.Get(m_AccountId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_AccountId);
		}
	}

	public Utf8String DisplayName
	{
		get
		{
			Helper.Get(m_DisplayName, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DisplayName);
		}
	}

	public Utf8String DisplayNameSanitized
	{
		get
		{
			Helper.Get(m_DisplayNameSanitized, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_DisplayNameSanitized);
		}
	}

	public void Set(ref ExternalUserInfo other)
	{
		m_ApiVersion = 2;
		AccountType = other.AccountType;
		AccountId = other.AccountId;
		DisplayName = other.DisplayName;
		DisplayNameSanitized = other.DisplayNameSanitized;
	}

	public void Set(ref ExternalUserInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			AccountType = other.Value.AccountType;
			AccountId = other.Value.AccountId;
			DisplayName = other.Value.DisplayName;
			DisplayNameSanitized = other.Value.DisplayNameSanitized;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_AccountId);
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_DisplayNameSanitized);
	}

	public void Get(out ExternalUserInfo output)
	{
		output = default(ExternalUserInfo);
		output.Set(ref this);
	}
}
