using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct TransactionCopyEntitlementByIndexOptionsInternal : ISettable<TransactionCopyEntitlementByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private uint m_EntitlementIndex;

	public uint EntitlementIndex
	{
		set
		{
			m_EntitlementIndex = value;
		}
	}

	public void Set(ref TransactionCopyEntitlementByIndexOptions other)
	{
		m_ApiVersion = 1;
		EntitlementIndex = other.EntitlementIndex;
	}

	public void Set(ref TransactionCopyEntitlementByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			EntitlementIndex = other.Value.EntitlementIndex;
		}
	}

	public void Dispose()
	{
	}
}
