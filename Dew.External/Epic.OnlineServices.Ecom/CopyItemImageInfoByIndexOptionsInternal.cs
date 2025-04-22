using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyItemImageInfoByIndexOptionsInternal : ISettable<CopyItemImageInfoByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_ItemId;

	private uint m_ImageInfoIndex;

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

	public uint ImageInfoIndex
	{
		set
		{
			m_ImageInfoIndex = value;
		}
	}

	public void Set(ref CopyItemImageInfoByIndexOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		ItemId = other.ItemId;
		ImageInfoIndex = other.ImageInfoIndex;
	}

	public void Set(ref CopyItemImageInfoByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			ItemId = other.Value.ItemId;
			ImageInfoIndex = other.Value.ImageInfoIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ItemId);
	}
}
