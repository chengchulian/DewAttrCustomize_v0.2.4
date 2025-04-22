using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyOfferImageInfoByIndexOptionsInternal : ISettable<CopyOfferImageInfoByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_OfferId;

	private uint m_ImageInfoIndex;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public Utf8String OfferId
	{
		set
		{
			Helper.Set(value, ref m_OfferId);
		}
	}

	public uint ImageInfoIndex
	{
		set
		{
			m_ImageInfoIndex = value;
		}
	}

	public void Set(ref CopyOfferImageInfoByIndexOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		OfferId = other.OfferId;
		ImageInfoIndex = other.ImageInfoIndex;
	}

	public void Set(ref CopyOfferImageInfoByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			OfferId = other.Value.OfferId;
			ImageInfoIndex = other.Value.ImageInfoIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_OfferId);
	}
}
