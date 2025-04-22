using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetProductUserIdMappingOptionsInternal : ISettable<GetProductUserIdMappingOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private ExternalAccountType m_AccountIdType;

	private IntPtr m_TargetProductUserId;

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

	public ProductUserId TargetProductUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetProductUserId);
		}
	}

	public void Set(ref GetProductUserIdMappingOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		AccountIdType = other.AccountIdType;
		TargetProductUserId = other.TargetProductUserId;
	}

	public void Set(ref GetProductUserIdMappingOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			AccountIdType = other.Value.AccountIdType;
			TargetProductUserId = other.Value.TargetProductUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetProductUserId);
	}
}
