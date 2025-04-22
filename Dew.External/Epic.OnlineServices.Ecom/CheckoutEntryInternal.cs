using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct CheckoutEntryInternal : IGettable<CheckoutEntry>, ISettable<CheckoutEntry>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_OfferId;

	public Utf8String OfferId
	{
		get
		{
			Helper.Get(m_OfferId, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_OfferId);
		}
	}

	public void Set(ref CheckoutEntry other)
	{
		m_ApiVersion = 1;
		OfferId = other.OfferId;
	}

	public void Set(ref CheckoutEntry? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			OfferId = other.Value.OfferId;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_OfferId);
	}

	public void Get(out CheckoutEntry output)
	{
		output = default(CheckoutEntry);
		output.Set(ref this);
	}
}
