using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct QueryOwnershipBySandboxIdsOptionsInternal : ISettable<QueryOwnershipBySandboxIdsOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_SandboxIds;

	private uint m_SandboxIdsCount;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String[] SandboxIds
	{
		set
		{
			Helper.Set(value, ref m_SandboxIds, out m_SandboxIdsCount);
		}
	}

	public void Set(ref QueryOwnershipBySandboxIdsOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		SandboxIds = other.SandboxIds;
	}

	public void Set(ref QueryOwnershipBySandboxIdsOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			SandboxIds = other.Value.SandboxIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_SandboxIds);
	}
}
