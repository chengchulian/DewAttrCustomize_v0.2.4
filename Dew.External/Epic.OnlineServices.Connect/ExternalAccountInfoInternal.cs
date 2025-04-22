using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ExternalAccountInfoInternal : IGettable<ExternalAccountInfo>, ISettable<ExternalAccountInfo>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_ProductUserId;

	private IntPtr m_DisplayName;

	private IntPtr m_AccountId;

	private ExternalAccountType m_AccountIdType;

	private long m_LastLoginTime;

	public ProductUserId ProductUserId
	{
		get
		{
			Helper.Get(m_ProductUserId, out ProductUserId value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_ProductUserId);
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

	public ExternalAccountType AccountIdType
	{
		get
		{
			return m_AccountIdType;
		}
		set
		{
			m_AccountIdType = value;
		}
	}

	public DateTimeOffset? LastLoginTime
	{
		get
		{
			Helper.Get(m_LastLoginTime, out var value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_LastLoginTime);
		}
	}

	public void Set(ref ExternalAccountInfo other)
	{
		m_ApiVersion = 1;
		ProductUserId = other.ProductUserId;
		DisplayName = other.DisplayName;
		AccountId = other.AccountId;
		AccountIdType = other.AccountIdType;
		LastLoginTime = other.LastLoginTime;
	}

	public void Set(ref ExternalAccountInfo? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			ProductUserId = other.Value.ProductUserId;
			DisplayName = other.Value.DisplayName;
			AccountId = other.Value.AccountId;
			AccountIdType = other.Value.AccountIdType;
			LastLoginTime = other.Value.LastLoginTime;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_ProductUserId);
		Helper.Dispose(ref m_DisplayName);
		Helper.Dispose(ref m_AccountId);
	}

	public void Get(out ExternalAccountInfo output)
	{
		output = default(ExternalAccountInfo);
		output.Set(ref this);
	}
}
