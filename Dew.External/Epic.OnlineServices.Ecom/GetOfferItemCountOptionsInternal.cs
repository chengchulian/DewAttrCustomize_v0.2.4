using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct GetOfferItemCountOptionsInternal : ISettable<GetOfferItemCountOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_LocalUserId;

	private IntPtr m_OfferId;

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

	public void Set(ref GetOfferItemCountOptions other)
	{
		m_ApiVersion = 1;
		LocalUserId = other.LocalUserId;
		OfferId = other.OfferId;
	}

	public void Set(ref GetOfferItemCountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			LocalUserId = other.Value.LocalUserId;
			OfferId = other.Value.OfferId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_LocalUserId);
		Helper.Dispose(ref m_OfferId);
	}
}
