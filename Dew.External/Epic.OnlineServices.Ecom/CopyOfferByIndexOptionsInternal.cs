using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CopyOfferByIndexOptionsInternal : ISettable<CopyOfferByIndexOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private uint m_OfferIndex;

	public EpicAccountId LocalUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalUserId);
		}
	}

	public uint OfferIndex
	{
		set
		{
			m_OfferIndex = value;
		}
	}

	public void Set(ref CopyOfferByIndexOptions other)
	{
		m_ApiVersion = 3;
		LocalUserId = other.LocalUserId;
		OfferIndex = other.OfferIndex;
	}

	public void Set(ref CopyOfferByIndexOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 3;
			LocalUserId = other.Value.LocalUserId;
			OfferIndex = other.Value.OfferIndex;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
	}
}
