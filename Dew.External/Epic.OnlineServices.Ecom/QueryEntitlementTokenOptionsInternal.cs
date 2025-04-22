using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryEntitlementTokenOptionsInternal : ISettable<QueryEntitlementTokenOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_EntitlementNames;

	private uint m_EntitlementNameCount;

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

	public void Set(ref QueryEntitlementTokenOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		EntitlementNames = other.EntitlementNames;
	}

	public void Set(ref QueryEntitlementTokenOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			EntitlementNames = other.Value.EntitlementNames;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_EntitlementNames);
	}
}
