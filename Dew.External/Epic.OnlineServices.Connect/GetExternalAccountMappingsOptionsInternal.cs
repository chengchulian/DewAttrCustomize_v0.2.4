using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetExternalAccountMappingsOptionsInternal : ISettable<GetExternalAccountMappingsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private ExternalAccountType m_AccountIdType;

	private IntPtr m_TargetExternalUserId;

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

	public Utf8String TargetExternalUserId
	{
		set
		{
			Helper.Set(value, ref m_TargetExternalUserId);
		}
	}

	public void Set(ref GetExternalAccountMappingsOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		AccountIdType = other.AccountIdType;
		TargetExternalUserId = other.TargetExternalUserId;
	}

	public void Set(ref GetExternalAccountMappingsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			AccountIdType = other.Value.AccountIdType;
			TargetExternalUserId = other.Value.TargetExternalUserId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_TargetExternalUserId);
	}
}
