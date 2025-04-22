using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct RedeemEntitlementsOptionsInternal : ISettable<RedeemEntitlementsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private uint m_EntitlementIdCount;

	private IntPtr m_EntitlementIds;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String[] EntitlementIds
	{
		set
		{
			Helper.Set(value, ref m_EntitlementIds, out m_EntitlementIdCount);
		}
	}

	public void Set(ref RedeemEntitlementsOptions other)
	{
		m_ApiVersion = 2;
		LocalUserId = other.LocalUserId;
		EntitlementIds = other.EntitlementIds;
	}

	public void Set(ref RedeemEntitlementsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 2;
			LocalUserId = other.Value.LocalUserId;
			EntitlementIds = other.Value.EntitlementIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_EntitlementIds);
	}
}
