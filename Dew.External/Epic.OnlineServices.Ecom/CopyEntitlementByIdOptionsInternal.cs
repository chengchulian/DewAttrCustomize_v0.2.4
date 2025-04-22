using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyEntitlementByIdOptionsInternal : ISettable<CopyEntitlementByIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_EntitlementId;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String EntitlementId
	{
		set
		{
			Helper.Set(value, ref m_EntitlementId);
		}
	}

	public void Set(ref CopyEntitlementByIdOptions other)
	{
		m_ApiVersion = 2;
		LocalUserId = other.LocalUserId;
		EntitlementId = other.EntitlementId;
	}

	public void Set(ref CopyEntitlementByIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LocalUserId = other.Value.LocalUserId;
			EntitlementId = other.Value.EntitlementId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_EntitlementId);
	}
}
