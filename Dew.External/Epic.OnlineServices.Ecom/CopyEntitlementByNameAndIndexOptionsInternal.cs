using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyEntitlementByNameAndIndexOptionsInternal : ISettable<CopyEntitlementByNameAndIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_EntitlementName;

	private uint m_Index;

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

	public uint Index
	{
		set
		{
			m_Index = value;
		}
	}

	public void Set(ref CopyEntitlementByNameAndIndexOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		EntitlementName = other.EntitlementName;
		Index = other.Index;
	}

	public void Set(ref CopyEntitlementByNameAndIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			EntitlementName = other.Value.EntitlementName;
			Index = other.Value.Index;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_EntitlementName);
	}
}
