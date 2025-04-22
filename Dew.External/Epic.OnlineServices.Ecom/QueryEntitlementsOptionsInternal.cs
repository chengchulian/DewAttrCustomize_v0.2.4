using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryEntitlementsOptionsInternal : ISettable<QueryEntitlementsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_EntitlementNames;

	private uint m_EntitlementNameCount;

	private int m_IncludeRedeemed;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String[] EntitlementNames
	{
		set
		{
			Helper.Set(value, ref m_EntitlementNames, out m_EntitlementNameCount);
		}
	}

	public bool IncludeRedeemed
	{
		set
		{
			Helper.Set(value, ref m_IncludeRedeemed);
		}
	}

	public void Set(ref QueryEntitlementsOptions other)
	{
		m_ApiVersion = 2;
		LocalUserId = other.LocalUserId;
		EntitlementNames = other.EntitlementNames;
		IncludeRedeemed = other.IncludeRedeemed;
	}

	public void Set(ref QueryEntitlementsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LocalUserId = other.Value.LocalUserId;
			EntitlementNames = other.Value.EntitlementNames;
			IncludeRedeemed = other.Value.IncludeRedeemed;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_EntitlementNames);
	}
}
