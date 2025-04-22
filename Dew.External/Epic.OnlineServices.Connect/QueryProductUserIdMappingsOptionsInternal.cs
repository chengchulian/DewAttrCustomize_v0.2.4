using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryProductUserIdMappingsOptionsInternal : ISettable<QueryProductUserIdMappingsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private ExternalAccountType m_AccountIdType_DEPRECATED;

	private IntPtr m_ProductUserIds;

	private uint m_ProductUserIdCount;

	public ProductUserId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public ExternalAccountType AccountIdType_DEPRECATED
	{
		set
		{
			m_AccountIdType_DEPRECATED = value;
		}
	}

	public ProductUserId[] ProductUserIds
	{
		set
		{
			Helper.Set(value, ref m_ProductUserIds, out m_ProductUserIdCount);
		}
	}

	public void Set(ref QueryProductUserIdMappingsOptions other)
	{
		m_ApiVersion = 2;
		LocalUserId = other.LocalUserId;
		AccountIdType_DEPRECATED = other.AccountIdType_DEPRECATED;
		ProductUserIds = other.ProductUserIds;
	}

	public void Set(ref QueryProductUserIdMappingsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LocalUserId = other.Value.LocalUserId;
			AccountIdType_DEPRECATED = other.Value.AccountIdType_DEPRECATED;
			ProductUserIds = other.Value.ProductUserIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ProductUserIds);
	}
}
