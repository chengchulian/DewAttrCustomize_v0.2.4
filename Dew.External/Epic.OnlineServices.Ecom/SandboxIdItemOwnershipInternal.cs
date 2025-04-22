using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct SandboxIdItemOwnershipInternal : IGettable<SandboxIdItemOwnership>, ISettable<SandboxIdItemOwnership>, IDisposable
{
	private IntPtr m_SandboxId;

	private IntPtr m_OwnedCatalogItemIds;

	private uint m_OwnedCatalogItemIdsCount;

	public Utf8String SandboxId
	{
		get
		{
			Helper.Get(m_SandboxId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_SandboxId);
		}
	}

	public Utf8String[] OwnedCatalogItemIds
	{
		get
		{
			Helper.Get(m_OwnedCatalogItemIds, out Utf8String[] value, m_OwnedCatalogItemIdsCount);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OwnedCatalogItemIds, out m_OwnedCatalogItemIdsCount);
		}
	}

	public void Set(ref SandboxIdItemOwnership other)
	{
		SandboxId = other.SandboxId;
		OwnedCatalogItemIds = other.OwnedCatalogItemIds;
	}

	public void Set(ref SandboxIdItemOwnership? other)
	{
		if (other.HasValue)
		{
			SandboxId = other.Value.SandboxId;
			OwnedCatalogItemIds = other.Value.OwnedCatalogItemIds;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_SandboxId);
		Helper.Dispose(ref m_OwnedCatalogItemIds);
	}

	public void Get(out SandboxIdItemOwnership output)
	{
		output = default(SandboxIdItemOwnership);
		output.Set(ref this);
	}
}
