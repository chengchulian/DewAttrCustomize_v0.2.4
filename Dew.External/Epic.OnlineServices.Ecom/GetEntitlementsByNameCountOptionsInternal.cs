using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetEntitlementsByNameCountOptionsInternal : ISettable<GetEntitlementsByNameCountOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_EntitlementName;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String EntitlementName
	{
		set
		{
			Helper.Set(value, ref m_EntitlementName);
		}
	}

	public void Set(ref GetEntitlementsByNameCountOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		EntitlementName = other.EntitlementName;
	}

	public void Set(ref GetEntitlementsByNameCountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			EntitlementName = other.Value.EntitlementName;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_EntitlementName);
	}
}
