using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
internal struct TransferDeviceIdAccountOptionsInternal : ISettable<TransferDeviceIdAccountOptions>, IDisposable
{
	private int m_ApiVersion;

	private IntPtr m_PrimaryLocalUserId;

	private IntPtr m_LocalDeviceUserId;

	private IntPtr m_ProductUserIdToPreserve;

	public ProductUserId PrimaryLocalUserId
	{
		set
		{
			Helper.Set(value, ref m_PrimaryLocalUserId);
		}
	}

	public ProductUserId LocalDeviceUserId
	{
		set
		{
			Helper.Set(value, ref m_LocalDeviceUserId);
		}
	}

	public ProductUserId ProductUserIdToPreserve
	{
		set
		{
			Helper.Set(value, ref m_ProductUserIdToPreserve);
		}
	}

	public void Set(ref TransferDeviceIdAccountOptions other)
	{
		m_ApiVersion = 1;
		PrimaryLocalUserId = other.PrimaryLocalUserId;
		LocalDeviceUserId = other.LocalDeviceUserId;
		ProductUserIdToPreserve = other.ProductUserIdToPreserve;
	}

	public void Set(ref TransferDeviceIdAccountOptions? other)
	{
		if (other.HasValue)
		{
			m_ApiVersion = 1;
			PrimaryLocalUserId = other.Value.PrimaryLocalUserId;
			LocalDeviceUserId = other.Value.LocalDeviceUserId;
			ProductUserIdToPreserve = other.Value.ProductUserIdToPreserve;
		}
	}

	public void Dispose()
	{
		Helper.Dispose(ref m_PrimaryLocalUserId);
		Helper.Dispose(ref m_LocalDeviceUserId);
		Helper.Dispose(ref m_ProductUserIdToPreserve);
	}
}
