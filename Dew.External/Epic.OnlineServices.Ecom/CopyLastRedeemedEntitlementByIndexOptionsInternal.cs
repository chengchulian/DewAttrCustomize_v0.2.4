using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyLastRedeemedEntitlementByIndexOptionsInternal : ISettable<CopyLastRedeemedEntitlementByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private uint m_RedeemedEntitlementIndex;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public uint RedeemedEntitlementIndex
	{
		set
		{
			m_RedeemedEntitlementIndex = value;
		}
	}

	public void Set(ref CopyLastRedeemedEntitlementByIndexOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		RedeemedEntitlementIndex = other.RedeemedEntitlementIndex;
	}

	public void Set(ref CopyLastRedeemedEntitlementByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			RedeemedEntitlementIndex = other.Value.RedeemedEntitlementIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
	}
}
