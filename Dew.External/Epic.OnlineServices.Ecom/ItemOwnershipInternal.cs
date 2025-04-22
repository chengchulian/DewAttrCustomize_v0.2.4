using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct ItemOwnershipInternal : IGettable<ItemOwnership>, ISettable<ItemOwnership>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_Id;

	private OwnershipStatus m_OwnershipStatus;

	public Utf8String Id
	{
		get
		{
			Helper.Get(m_Id, out Utf8String value);
			return value;
		}
		set
		{
			Helper.Set(value, ref m_Id);
		}
	}

	public OwnershipStatus OwnershipStatus
	{
		get
		{
			return m_OwnershipStatus;
		}
		set
		{
			m_OwnershipStatus = value;
		}
	}

	public void Set(ref ItemOwnership other)
	{
		m_ApiVersion = 1;
		Id = other.Id;
		OwnershipStatus = other.OwnershipStatus;
	}

	public void Set(ref ItemOwnership? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			Id = other.Value.Id;
			OwnershipStatus = other.Value.OwnershipStatus;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_Id);
	}

	public void Get(out ItemOwnership output)
	{
		output = default(ItemOwnership);
		output.Set(ref this);
	}
}
