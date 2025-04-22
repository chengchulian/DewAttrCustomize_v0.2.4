using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryExternalAccountMappingsOptionsInternal : ISettable<QueryExternalAccountMappingsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private ExternalAccountType m_AccountIdType;

	private IntPtr m_ExternalAccountIds;

	private uint m_ExternalAccountIdCount;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public ExternalAccountType AccountIdType
	{
		set
		{
			m_AccountIdType = value;
		}
	}

	public Utf8String[] ExternalAccountIds
	{
		set
		{
			Helper.Set(value, ref m_ExternalAccountIds, isArrayItemAllocated: true, out m_ExternalAccountIdCount);
		}
	}

	public void Set(ref QueryExternalAccountMappingsOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		AccountIdType = other.AccountIdType;
		ExternalAccountIds = other.ExternalAccountIds;
	}

	public void Set(ref QueryExternalAccountMappingsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			AccountIdType = other.Value.AccountIdType;
			ExternalAccountIds = other.Value.ExternalAccountIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ExternalAccountIds);
	}
}
