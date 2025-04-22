using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyItemReleaseByIndexOptionsInternal : ISettable<CopyItemReleaseByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_ItemId;

	private uint m_ReleaseIndex;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String ItemId
	{
		set
		{
			Helper.Set(value, ref m_ItemId);
		}
	}

	public uint ReleaseIndex
	{
		set
		{
			m_ReleaseIndex = value;
		}
	}

	public void Set(ref CopyItemReleaseByIndexOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		ItemId = other.ItemId;
		ReleaseIndex = other.ReleaseIndex;
	}

	public void Set(ref CopyItemReleaseByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			ItemId = other.Value.ItemId;
			ReleaseIndex = other.Value.ReleaseIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ItemId);
	}
}
