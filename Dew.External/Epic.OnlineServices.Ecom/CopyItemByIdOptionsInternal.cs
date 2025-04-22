using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyItemByIdOptionsInternal : ISettable<CopyItemByIdOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_ItemId;

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

	public void Set(ref CopyItemByIdOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		ItemId = other.ItemId;
	}

	public void Set(ref CopyItemByIdOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			ItemId = other.Value.ItemId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_ItemId);
	}
}
